using ExpressedRealms.Expressions.UseCases.StatModifiers.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.StatModifiers.Delete;

internal static class DeleteStatModifierEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> ExecuteAsync(
        int groupId,
        int mappingId,
        IDeleteStatModifierUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteStatModifierModel() { Id = mappingId, StatModifierGroupId = groupId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
