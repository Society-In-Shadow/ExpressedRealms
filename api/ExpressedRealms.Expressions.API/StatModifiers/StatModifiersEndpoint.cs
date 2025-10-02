using ExpressedRealms.Authentication;
using ExpressedRealms.Expressions.API.StatModifiers.Create;
using ExpressedRealms.Expressions.API.StatModifiers.Delete;
using ExpressedRealms.Expressions.API.StatModifiers.Edit;
using ExpressedRealms.Expressions.API.StatModifiers.Get;
using ExpressedRealms.Expressions.API.StatModifiers.GetModifierTypes;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;

namespace ExpressedRealms.Expressions.API.StatModifiers;

internal static class StatModifiersEndpoint
{
    internal static void AddStatModifiersEndpoints(this WebApplication app)
    {
        var endpointGroup = app.MapGroup("modifiergroups")
            .AddFluentValidationAutoValidation()
            .WithTags("Stat Modifier Groups")
            .WithOpenApi();

        endpointGroup.MapGet("{groupId}/modifiers", GetStatModifiersEndpoint.ExecuteAsync);

        endpointGroup
            .MapGet("/modifiers/options", GetStatModifierTypesEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageModifiers);

        endpointGroup
            .MapPut("{groupId}/modifiers/{mappingId}", EditStatModifierEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageModifiers);

        endpointGroup
            .MapPost("{groupId:int?}/modifiers", CreateStatModifierEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageModifiers);
        
        endpointGroup
            .MapPost("/modifiers", CreateStatModifierNoGroupEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageModifiers);

        endpointGroup
            .MapDelete("{groupId}/modifiers/{mappingId}", DeleteStatModifierEndpoint.ExecuteAsync)
            .RequirePolicyAuthorization(Policies.ManageModifiers);
    }
}
