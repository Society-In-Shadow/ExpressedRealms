using ExpressedRealms.Expressions.API.FactionEndpoints.CreateFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.DeleteFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.EditFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.GetFaction;
using ExpressedRealms.Expressions.API.FactionEndpoints.GetFactions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Expressions.API.FactionEndpoints;

internal static class FactionEndpoints
{
    internal static void AddFactionEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("factions").WithTags("Factions").RequireAuthorization();

        endpointGroup.MapGet("", GetFactionsEndpoint.ExecuteAsync);

        endpointGroup.MapGet("{id}", GetFactionEndpoint.ExecuteAsync);

        endpointGroup.MapPost("", CreateFactionEndpoint.ExecuteAsync);

        endpointGroup.MapPut("{id}", EditFactionEndpoint.ExecuteAsync);

        endpointGroup.MapDelete("{id}", DeleteFactionEndpoint.ExecuteAsync);
    }
}
