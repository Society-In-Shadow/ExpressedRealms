using ExpressedRealms.Admin.API.AdminEndpoints.Dtos;
using ExpressedRealms.Admin.API.AdminEndpoints.GetRolesForUser;
using ExpressedRealms.Admin.API.AdminEndpoints.GetUsers;
using ExpressedRealms.Admin.API.AdminEndpoints.GetUserSummary;
using ExpressedRealms.Admin.API.AdminEndpoints.Request;
using ExpressedRealms.Admin.API.AdminEndpoints.Response;
using ExpressedRealms.Admin.API.AdminEndpoints.UpdateUserRoles;
using ExpressedRealms.Admin.Repository.ActivityLogs;
using ExpressedRealms.Authentication;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Admin.API.AdminEndpoints;

public static class AdminEndpoints
{
    internal static void AddAdminEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("admin")
            .AddFluentValidationAutoValidation()
            .WithTags("admin")
            .RequirePolicyAuthorization(Policies.UserManagementPolicy);

        endpointGroup.MapGet("users", GetUsersEndpoint.Execute).RequireAuthorization();

        endpointGroup
            .MapGet("users/summary", GetUserSummaryEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup
            .MapGet("users/{userId}/roles", GetRolesForUserEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup
            .MapPut("user/{userid}/role", UpdateUserRoleEndpoint.Execute)
            .RequireAuthorization();

        endpointGroup
            .MapPost(
                "user/{userId}/bypassEmailConfirmation",
                async Task<Results<NoContent, NotFound>> (
                    string userId,
                    UserManager<User> userManager
                ) =>
                {
                    var user = await userManager.FindByIdAsync(userId);

                    if (user == null)
                    {
                        return TypedResults.NotFound();
                    }

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    await userManager.ConfirmEmailAsync(user, token);

                    return TypedResults.NoContent();
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "user/{userid}/roles",
                async Task<Results<NoContent, NotFound, Ok<UserRoleResponse>>> (
                    Guid userId,
                    RoleManager<Role> roleManager,
                    UserManager<User> userManager
                ) =>
                {
                    var user = await userManager.FindByIdAsync(userId.ToString());

                    if (user == null)
                    {
                        return TypedResults.NotFound();
                    }

                    var allRoles = await roleManager.Roles.ToListAsync();

                    var userRoles = await userManager.GetRolesAsync(user);

                    var roles = allRoles
                        .Select(x => new UserRoleDto()
                        {
                            Name = x.Name!,
                            IsEnabled = userRoles.Any(y => y == x.Name),
                        })
                        .ToList();

                    return TypedResults.Ok(new UserRoleResponse() { Roles = roles });
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapGet(
                "user/{userid}/activitylogs",
                async Task<Ok<LogResponse>> (Guid userId, IActivityLogRepository repository) =>
                {
                    var userLogs = await repository.GetUserLogs(userId.ToString());

                    return TypedResults.Ok(
                        new LogResponse()
                        {
                            Logs = userLogs
                                .Select(
                                    (x, index) =>
                                        new LogDto()
                                        {
                                            Id = index,
                                            ChangedProperties = x.ChangedProperties,
                                            Location = x.Location,
                                            TimeStamp = x.TimeStamp,
                                            Action = x.Action,
                                        }
                                )
                                .ToList(),
                        }
                    );
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "user/{userid}/lockout",
                async Task<Results<NoContent, NotFound, BadRequest<string>>> (
                    string userId,
                    DisableUserRequest dto,
                    UserManager<User> userManager
                ) =>
                {
                    var user = await userManager.FindByIdAsync(dto.UserId);

                    if (user == null)
                    {
                        return TypedResults.NotFound();
                    }

                    var expireDate = dto.CustomExpiryDate ?? DateTime.MaxValue;
                    if (!dto.LockoutEnabled)
                        expireDate = DateTime.UtcNow;

                    await userManager.SetLockoutEndDateAsync(user, expireDate);

                    return TypedResults.NoContent();
                }
            )
            .RequireAuthorization();
    }
}
