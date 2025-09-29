using ExpressedRealms.Expressions.UseCases.ProgressionPaths.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Delete;

internal static class DeleteProgressionPathEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> ExecuteAsync(
        int expressionId,
        int progressionId,
        IDeleteProgressionPathUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteProgressionPathModel() { Id = progressionId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
