using ExpressedRealms.Authentication;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
using ExpressedRealms.Repositories.Admin;
using ExpressedRealms.Server.EndPoints.AdminEndpoints.Dtos;
using ExpressedRealms.Server.EndPoints.AdminEndpoints.Request;
using ExpressedRealms.Server.EndPoints.AdminEndpoints.Response;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Server.EndPoints.AdminEndpoints;

public static class AdminEndpoints
{
    internal static void AddAdminEndPoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("admin")
            .AddFluentValidationAutoValidation()
            .WithTags("admin")
            .RequirePolicyAuthorization(Policies.UserManagementPolicy)
            .WithOpenApi();

        endpointGroup
            .MapGet(
                "users",
                [Authorize]
                async Task<Ok<UserListResponse>> (IUsersRepository repository) =>
                {
                    var users = await repository.GetUsersAsync();

                    return TypedResults.Ok(
                        new UserListResponse()
                        {
                            Users = users
                                .Select(x => new UserListItem()
                                {
                                    Id = x.Id,
                                    Email = x.Email,
                                    Username = x.Username,
                                    Roles = x.Roles,
                                    IsDisabled = x.IsDisabled,
                                    LockedOut = x.LockedOut,
                                    LockedOutExpires = x.LockOutExpires,
                                })
                                .OrderBy(x => x.Email)
                                .ToList(),
                        }
                    );
                }
            )
            .RequireAuthorization();

        endpointGroup
            .MapPut(
                "user/{userid}/role",
                async Task<Results<NoContent, NotFound, BadRequest<string>>> (
                    string userId,
                    UpdateUserRoleRequest dto,
                    UserManager<User> userManager,
                    RoleManager<Role> roleManager,
                    SignInManager<User> signInManager
                ) =>
                {
                    var user = await userManager.FindByIdAsync(dto.UserId);

                    if (user == null)
                    {
                        return TypedResults.NotFound();
                    }

                    // Ensure the role exists before assigning
                    if (!await roleManager.RoleExistsAsync(dto.RoleName))
                    {
                        return TypedResults.NotFound();
                    }

                    if (dto.IsEnabled)
                    {
                        var result = await userManager.AddToRoleAsync(user, dto.RoleName);
                        if (result.Succeeded)
                        {
                            return TypedResults.NoContent();
                        }
                    }
                    else
                    {
                        var result = await userManager.RemoveFromRoleAsync(user, dto.RoleName);
                        if (result.Succeeded)
                        {
                            return TypedResults.NoContent();
                        }
                    }

                    await signInManager.RefreshSignInAsync(user);

                    return TypedResults.BadRequest<string>("The role was not updated.");
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
                            Name = x.Name,
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
