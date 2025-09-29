using ExpressedRealms.Expressions.UseCases.ProgressionLevels.Add;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionLevels.Create;

internal static class CreateProgressionLevelEndpoint
{
    public static async Task<Results<ValidationProblem, Created<int>>> ExecuteAsync(
        int expressionId,
        int progressionId,
        CreateProgressionLevel request,
        IAddProgressionLevelUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new AddProgressionLevelModel()
            {
                ProgressionId = progressionId,
                XlLevel = request.XlLevel,
                Description = request.Description,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Created($"/expressions/{expressionId}/progression/{progressionId}/level/", results.Value);
    }
}
