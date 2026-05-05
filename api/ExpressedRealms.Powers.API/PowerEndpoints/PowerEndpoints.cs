using ExpressedRealms.Authentication;
using ExpressedRealms.Powers.API.PowerEndpoints.CreatePower;
using ExpressedRealms.Powers.API.PowerEndpoints.DeletePower;
using ExpressedRealms.Powers.API.PowerEndpoints.EditPower;
using ExpressedRealms.Powers.API.PowerEndpoints.GetPower;
using ExpressedRealms.Powers.API.PowerEndpoints.GetPowerOptions;
using ExpressedRealms.Powers.API.PowerEndpoints.GetPowersForPowerPath;
using ExpressedRealms.Powers.API.PowerEndpoints.UpdatePowerPathSorting;
using ExpressedRealms.Server.Shared;
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
            .MapGet("/{id}/powers", GetPowersForPowerPathEndpoint.Execute)
            .WithSummary("Returns the list of powers for a given power path")
            .WithDescription(" of powers for a given power path");

        app.MapGroup("powerpath")
            .WithTags("Power Paths")
            .MapPut(
                "/{powerPathId}/updateSorting",
                UpdatePowerPathPowerSortingEndpoint.Execute
            )
            .WithSummary("Updates the sort order of power paths for a given expression")
            .RequirePolicyAuthorization(Policies.ManagePowers);

        endpointGroup
            .MapGet("/{powerId}", GetPowerEndpoint.Execute)
            .WithSummary("Returns the specified power for a given expression for editing purposes")
            .WithDescription(" of powers for a given expression");

        endpointGroup
            .MapGet("/options", GetPowerOptionsEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .WithSummary("Returns available options for powers")
            .WithDescription(
                "This endpoint retrieves the available options for creating or editing powers."
            );

        endpointGroup
            .MapPost("", CreatePowerEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .WithSummary("Allows one to create new powers");

        endpointGroup
            .MapPut("{id}", EditPowerEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManagePowers)
            .WithSummary("Allows one to edit new powers");

        endpointGroup
            .MapDelete("{id}", DeletePowerEndpoint.Execute)
            .RequirePolicyAuthorization(Policies.ManagePowers);
    }
}
