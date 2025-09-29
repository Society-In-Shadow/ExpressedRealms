using ExpressedRealms.Expressions.UseCases.ProgressionLevels.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Edit;

internal static class EditProgressionLevelEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> ExecuteAsync(
        int expressionId,
        int pathId,
        int levelId,
        EditProgressionLevel request,
        IEditProgressionLevelUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditProgressionLevelModel()
            {
                ProgressionPathId = pathId,
                ProgressionLevelId = levelId,
                Description = request.Description,
                XlLevel = request.XlLevel
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
