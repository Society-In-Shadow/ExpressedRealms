using ExpressedRealms.Authentication.PermissionCollection;
using ExpressedRealms.Authentication.PermissionCollection.Configuration;
using ExpressedRealms.Expressions.API.FactionEndpoints.CreateFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.DeleteFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.EditFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.GetFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.GetFactions;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Expressions.API.FactionEndpoints;

internal static class FactionEndpoints
{
    internal static void AddFactionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("factions")
            .WithTags("Factions")
            .RequireAuthorization()
            .RequireFeatureToggle(ReleaseFlags.ShowFactions);

        endpointGroup.MapGet("expressions/{expressionId}", GetFactionsEndpoint.ExecuteAsync);

        endpointGroup
            .MapGet("{id}", GetFactionEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Faction.View);

        endpointGroup
            .MapPost("", CreateFactionEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Faction.Create);

        endpointGroup
            .MapPut("{id}", EditFactionEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Faction.Edit);

        endpointGroup
            .MapDelete("{id}", DeleteFactionEndpoint.ExecuteAsync)
            .RequirePermission(Permissions.Faction.Delete);
    }
}
