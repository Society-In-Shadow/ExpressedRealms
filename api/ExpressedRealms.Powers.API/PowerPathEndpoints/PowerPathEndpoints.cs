using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Powers.API.PowerPathEndpoints.CreatePowerPath;
using ExpressedRealms.Powers.API.PowerPathEndpoints.DeletePowerPath;
using ExpressedRealms.Powers.API.PowerPathEndpoints.DownloadPowerBooklet;
using ExpressedRealms.Powers.API.PowerPathEndpoints.DownloadPowerCards;
using ExpressedRealms.Powers.API.PowerPathEndpoints.EditPowerPath;
using ExpressedRealms.Powers.API.PowerPathEndpoints.GetPowerPath;
using ExpressedRealms.Powers.API.PowerPathEndpoints.GetPowerPathsForExpression;
using ExpressedRealms.Powers.API.PowerPathEndpoints.UpdatePowerPathSorting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints;

internal static class PowerPathEndpoints
{
    internal static void AddPowerPathApi(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("powerpath")
            .WithTags("Power Paths");

        var expressionGroup = app.MapGroup("expression")
            .WithTags("Expressions");

        expressionGroup
            .MapGet("/{expressionId}/powerPaths", GetPowerPathsForExpressionEndpoint.Execute)
            .RequireAuthorization();

        expressionGroup
            .MapGet("/{expressionId}/getPowerCards", DownloadPowerCardsEndpoint.Execute)
            .WithDescription("Downloads the power cards for the given expression")
            .RequirePermission(Permissions.Powers.DownloadAllPowerCards);

        expressionGroup
            .MapGet("/{expressionId}/powerBooklet", DownloadPowerBookletEndpoint.Execute)
            .WithDescription("Downloads the power cards for the given expression")
            .RequirePermission(Permissions.Powers.DownloadBooklet);

        expressionGroup
            .MapPut("/{expressionId}/updateSorting", UpdatePowerPathSortingEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapGet("/{id}", GetPowerPathEndpoint.Execute)
            .RequirePermission(Permissions.Powers.View);

        endpointGroup
            .MapPost("", CreatePowerPathEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapPut("{id}", EditPowerPathEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);

        endpointGroup
            .MapDelete("{id}", DeletePowerPathEndpoint.Execute)
            .RequirePermission(Permissions.Powers.Edit);
    }
}
