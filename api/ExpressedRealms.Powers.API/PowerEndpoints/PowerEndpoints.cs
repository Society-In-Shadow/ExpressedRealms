using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Powers.API.PowerEndpoints.CreatePower;
using ExpressedRealms.Powers.API.PowerEndpoints.DeletePower;
using ExpressedRealms.Powers.API.PowerEndpoints.EditPower;
using ExpressedRealms.Powers.API.PowerEndpoints.GetPower;
using ExpressedRealms.Powers.API.PowerEndpoints.GetPowerOptions;
using ExpressedRealms.Powers.API.PowerEndpoints.GetPowersForPowerPath;
using ExpressedRealms.Powers.API.PowerEndpoints.UpdatePowerPathSorting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerEndpoints;

internal static class PowerEndpoints
{
    internal static void AddPowerApi(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("powers")
            .RequireAuthorization()
            .WithTags("Powers");

        app.MapGroup("/powerpath/")
            .WithTags("Power Paths")
            .MapGet("/{id}/powers", GetPowersForPowerPathEndpoint.Execute);

        app.MapGroup("powerpath")
            .WithTags("Power Paths")
            .MapPut(
                "/{powerPathId}/updateSorting",
                UpdatePowerPathPowerSortingEndpoint.Execute
            )
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapGet("/{powerId}", GetPowerEndpoint.Execute)
            .RequirePermission(Permissions.Powers.View);

        endpointGroup
            .MapGet("/options", GetPowerOptionsEndpoint.Execute)
            .RequirePermission(Permissions.Powers.View);

        endpointGroup
            .MapPost("", CreatePowerEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Create);

        endpointGroup
            .MapPut("{id}", EditPowerEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapDelete("{id}", DeletePowerEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Delete);
    }
}
