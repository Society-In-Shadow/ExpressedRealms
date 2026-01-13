using ExpressedRealms.Admin.API.RolesEndpoints.AddRole;
using ExpressedRealms.Admin.API.RolesEndpoints.AddUserRole;
using ExpressedRealms.Admin.API.RolesEndpoints.DeleteRole;
using ExpressedRealms.Admin.API.RolesEndpoints.DeleteUserRoleMapping;
using ExpressedRealms.Admin.API.RolesEndpoints.EditRole;
using ExpressedRealms.Admin.API.RolesEndpoints.GetPermissions;
using ExpressedRealms.Admin.API.RolesEndpoints.GetRole;
using ExpressedRealms.Admin.API.RolesEndpoints.GetRoles;
using ExpressedRealms.Admin.API.RolesEndpoints.GetRoleSummary;
using ExpressedRealms.Admin.API.RolesEndpoints.GetUsersForRole;
using ExpressedRealms.Authentication;
using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Admin.API.RolesEndpoints;

public static class RolesEndpoints
{
    internal static void AddRolesEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("admin")
            .AddFluentValidationAutoValidation()
            .WithTags("Admin Roles List")
            .RequireAuthorization()
            .RequirePolicyAuthorization(Policies.UserManagementPolicy);

        endpointGroup
            .MapGet("roles/", GetRoleListEndpoint.Execute)
            .RequirePermission(Permissions.Role.View);

        endpointGroup
            .MapGet("roles/summary", GetRoleSummaryEndpoint.Execute)
            .RequirePermission(Permissions.Role.View);

        endpointGroup
            .MapGet("roles/{id}", GetRoleEndpoint.Execute)
            .RequirePermission(Permissions.Role.View);

        endpointGroup
            .MapGet("roles/permissions", GetPermissionsEndpoint.Execute)
            .RequirePermission(Permissions.Role.View);

        endpointGroup
            .MapPost("roles/", AddRoleEndpoint.Execute)
            .RequirePermission(Permissions.Role.Create);

        endpointGroup
            .MapPut("roles/{id}", EditRoleEndpoint.Execute)
            .RequirePermission(Permissions.Role.Edit);

        endpointGroup
            .MapDelete("roles/{id}", DeleteRoleEndpoint.Execute)
            .RequirePermission(Permissions.Role.Delete);

        endpointGroup
            .MapPost("roles/{id}/users", AddUserRoleEndpoint.Execute)
            .RequirePermission(Permissions.Role.Assign);

        endpointGroup
            .MapGet("roles/{id}/users", GetUsersForRoleEndpoint.Execute)
            .RequirePermission(Permissions.Role.Assign);

        endpointGroup
            .MapDelete("roles/{id}/users/{userId}", DeleteUserRoleMappingEndpoint.Execute)
            .RequirePermission(Permissions.Role.Assign);
    }
}
