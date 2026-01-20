using ExpressedRealms.Admin.API.AdminEndpoints.AddRoleUser;
using ExpressedRealms.Admin.API.AdminEndpoints.BypassEmailConfirmation;
using ExpressedRealms.Admin.API.AdminEndpoints.DeleteRoleUserMapping;
using ExpressedRealms.Admin.API.AdminEndpoints.DisableUser;
using ExpressedRealms.Admin.API.AdminEndpoints.Dtos;
using ExpressedRealms.Admin.API.AdminEndpoints.EnableUser;
using ExpressedRealms.Admin.API.AdminEndpoints.GetRolesForUser;
using ExpressedRealms.Admin.API.AdminEndpoints.GetUsers;
using ExpressedRealms.Admin.API.AdminEndpoints.GetUserSummary;
using ExpressedRealms.Admin.API.AdminEndpoints.Response;
using ExpressedRealms.Admin.API.AdminEndpoints.UnlockUser;
using ExpressedRealms.Admin.API.AdminEndpoints.UpdateUserRoles;
using ExpressedRealms.Admin.API.AdminEndpoints.ViewActivityLogs;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;
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
            .WithTags("admin");

        endpointGroup
            .MapGet("users", GetUsersEndpoint.Execute)
            .RequirePermission(Permissions.Player.View);

        endpointGroup
            .MapGet("users/summary", GetUserSummaryEndpoint.Execute)
            .RequirePermission(Permissions.Player.View);

        endpointGroup
            .MapGet("users/{userId}/roles", GetRolesForUserEndpoint.Execute)
            .RequirePermission(Permissions.Player.ManageRoles);

        endpointGroup
            .MapPost("users/{userid}/roles", AddRoleUserEndpoint.Execute)
            .RequirePermission(Permissions.Player.ManageRoles);
        
        endpointGroup
            .MapDelete("users/{userid}/roles/{roleId}", DeleteRoleUserMappingEndpoint.Execute)
            .RequirePermission(Permissions.Player.ManageRoles);
        
        endpointGroup
            .MapPut("user/{userid}/role", UpdateUserRoleEndpoint.Execute)
            .RequirePermission(Permissions.Player.ManageRoles);

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
            .RequirePermission(Permissions.Player.ManageRoles);

        endpointGroup
            .MapGet("user/{userid}/activitylogs", ViewActivityLogsEndpoint.Execute)
            .RequirePermission(Permissions.Player.ViewActivityLogs);

        endpointGroup
            .MapPut("user/{userid}/lockout", UnlockUserEndpoint.Execute)
            .RequirePermission(Permissions.Player.BypassLockout);

        endpointGroup
            .MapPut("user/{userid}/enable", EnableUserEndpoint.Execute)
            .RequirePermission(Permissions.Player.Enable);

        endpointGroup
            .MapDelete("user/{userid}", DisableUserEndpoint.Execute)
            .RequirePermission(Permissions.Player.Disable);

        endpointGroup
            .MapPost(
                "user/{userId}/bypassEmailConfirmation",
                BypassEmailConfirmationEndpoint.Execute
            )
            .RequirePermission(Permissions.Player.BypassEmailConfirmation);
    }
}
