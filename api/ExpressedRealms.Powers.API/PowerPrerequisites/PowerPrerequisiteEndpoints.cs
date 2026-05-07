using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Powers.API.PowerPrerequisites.CreatePrerequisite;
using ExpressedRealms.Powers.API.PowerPrerequisites.DeletePrerequisite;
using ExpressedRealms.Powers.API.PowerPrerequisites.EditPrerequisite;
using ExpressedRealms.Powers.API.PowerPrerequisites.GetPrerequisite;
using ExpressedRealms.Powers.API.PowerPrerequisites.GetPrerequisiteOptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerPrerequisites;

internal static class PowerPrerequisiteEndpoints
{
    internal static void AddPowerPrerequisiteApi(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("powers").WithTags("Powers - Prerequisites");

        endpointGroup
            .MapGet("/{powerId}/prerequisites", GetPrerequisiteEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapPost("/{powerId}/prerequisite", CreatePrerequisiteEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapDelete(
                "/{powerId}/prerequisite/{prerequisiteId}",
                DeletePrerequisiteEndpoint.Execute
            )
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapPut("/{powerId}/prerequisite/{prerequisiteId}", EditPrerequisiteEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);

        app.MapGroup("/powerpath/")
            .WithTags("Power Paths")
            .MapGet("/{id}/powerprerequisites/options", GetPrerequisiteOptionsEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);
    }
}
