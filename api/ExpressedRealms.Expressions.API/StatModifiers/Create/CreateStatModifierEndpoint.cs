using ExpressedRealms.Expressions.API.StatModifiers.StatModifiers.Create;
using ExpressedRealms.Expressions.UseCases.StatModifiers.Add;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.StatModifiers.Create;

internal static class CreateStatModifierEndpoint
{
    public static async Task<Results<ValidationProblem, Created<int>>> ExecuteAsync(
        int? groupId,
        CreateStatModifier request,
        IAddStatModifierUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new AddStatModifierModel()
            {
                Modifier = request.Modifier,
                ScaleWithLevel = request.ScaleWithLevel,
                CreationSpecificBonus = request.CreationSpecificBonus,
                SourceId = request.SourceId,
                SourceTable = request.SourceTable,
                StatModifierGroupId = groupId,
                StatModifierId = request.StatModifierId
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Created($"/modifiergroups/{results.Value.GroupId}/modifiers/", results.Value.ModifierMappingId);
    }
}
