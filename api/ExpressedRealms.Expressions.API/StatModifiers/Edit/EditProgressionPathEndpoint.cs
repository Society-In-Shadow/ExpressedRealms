using ExpressedRealms.Expressions.UseCases.StatModifiers.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.StatModifiers.Edit;

internal static class EditStatModifierEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> ExecuteAsync(
        int groupId,
        int mappingId,
        EditStatModifier request,
        IEditStatModifierUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditStatModifierModel()
            {
                Id = mappingId,
                Modifier = request.Modifier,
                ScaleWithLevel = request.ScaleWithLevel,
                StatModifierId = request.StatModifierId,
                StatModifierGroupId = groupId,
                CreationSpecificBonus = request.CreationSpecificBonus,
                TargetExpressionId = request.TargetExpressionId,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
