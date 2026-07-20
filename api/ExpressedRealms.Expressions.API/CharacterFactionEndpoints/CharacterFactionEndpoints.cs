using ExpressedRealms.Expressions.API.CharacterFactionEndpoints.JoinFaction;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints;

internal static class CharacterFactionMappingEndpoints
{
    internal static void AddFactionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("characters")
            .WithTags("Factions")
            .RequireAuthorization()
            .RequireFeatureToggle(ReleaseFlags.ShowFactions);

        /*endpointGroup.MapGet("expressions/{expressionId}", GetFactionsEndpoint.ExecuteAsync);*/

        endpointGroup.MapPost("{characterId}/factions/{factionId}", JoinFactionEndpoint.ExecuteAsync);
    }
}
