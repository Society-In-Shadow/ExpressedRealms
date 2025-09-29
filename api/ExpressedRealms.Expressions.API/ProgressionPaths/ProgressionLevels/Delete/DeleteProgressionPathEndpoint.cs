using ExpressedRealms.Expressions.UseCases.ProgressionLevels.Delete;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Delete;

internal static class DeleteProgressionLevelEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> ExecuteAsync(
        int expressionId,
        int progressionId,
        int levelId,
        IDeleteProgressionLevelUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new DeleteProgressionLevelModel()
            {
                ProgressionPathId = progressionId,
                ProgressionLevelId = levelId,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
