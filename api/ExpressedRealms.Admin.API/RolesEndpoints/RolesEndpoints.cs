using ExpressedRealms.Admin.API.RolesEndpoints.GetRoles;
using ExpressedRealms.Authentication;
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
            .RequirePolicyAuthorization(Policies.UserManagementPolicy);

        endpointGroup.MapGet("roles/", GetRoleListEndpoint.Execute).RequireAuthorization();
    }
}
