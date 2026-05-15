using ExpressedRealms.Admin.API.AdminEndpoints.AddRoleUser;
using ExpressedRealms.Admin.API.AdminEndpoints.BypassEmailConfirmation;
using ExpressedRealms.Admin.API.AdminEndpoints.DeleteRoleUserMapping;
using ExpressedRealms.Admin.API.AdminEndpoints.DisableUser;
using ExpressedRealms.Admin.API.AdminEndpoints.EnableUser;
using ExpressedRealms.Admin.API.AdminEndpoints.GetPlayer;
using ExpressedRealms.Admin.API.AdminEndpoints.GetRolesForUser;
using ExpressedRealms.Admin.API.AdminEndpoints.GetUsers;
using ExpressedRealms.Admin.API.AdminEndpoints.GetUserSummary;
using ExpressedRealms.Admin.API.AdminEndpoints.UnlockUser;
using ExpressedRealms.Admin.API.AdminEndpoints.UpdatePlayer;
using ExpressedRealms.Admin.API.AdminEndpoints.ViewActivityLogs;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
            .MapGet("user/{userid}/activitylogs", ViewActivityLogsEndpoint.Execute)
            .RequirePermission(Permissions.Player.ViewActivityLogs);

        endpointGroup
            .MapGet("user/{userid}/", GetPlayerEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Player.View);

        endpointGroup
            .MapPut("user/{userid}/", UpdatePlayerEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Player.Edit);

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
