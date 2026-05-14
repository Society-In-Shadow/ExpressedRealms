using ExpressedRealms.Expressions.API.StatModifiers.Create;
using ExpressedRealms.Expressions.API.StatModifiers.Delete;
using ExpressedRealms.Expressions.API.StatModifiers.Edit;
using ExpressedRealms.Expressions.API.StatModifiers.Get;
using ExpressedRealms.Expressions.API.StatModifiers.GetModifierTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ExpressedRealms.Expressions.API.StatModifiers;

internal static class StatModifiersEndpoint
{
    internal static void AddStatModifiersEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("modifiergroups")
            .WithTags("Stat Modifier Groups");

        endpointGroup.MapGet("{typeName}/{groupId}/modifiers", GetStatModifiersEndpoint.ExecuteAsync);

        endpointGroup
            .MapGet("{typeName}/modifiers/options", GetStatModifierTypesEndpoint.ExecuteAsync);

        endpointGroup
            .MapPut("{typeName}/{groupId}/modifiers/{mappingId}", EditStatModifierEndpoint.ExecuteAsync);

        endpointGroup
            .MapPost("{typeName}/{groupId:int?}/modifiers", CreateStatModifierEndpoint.ExecuteAsync);

        endpointGroup
            .MapPost("{typeName}/modifiers", CreateStatModifierNoGroupEndpoint.ExecuteAsync);

        endpointGroup
            .MapDelete("{typeName}/{groupId}/modifiers/{mappingId}", DeleteStatModifierEndpoint.ExecuteAsync);
    }
}
