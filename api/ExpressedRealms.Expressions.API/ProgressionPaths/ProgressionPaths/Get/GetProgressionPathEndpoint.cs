using ExpressedRealms.Expressions.UseCases.ProgressionPaths.GetPathsAndLevels;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Get;

internal static class GetProgressionPathsEndpoint
{
    public static async Task<Results<ValidationProblem, Ok<ProgressionPathsResponse>>> ExecuteAsync(
        int expressionId,
        IGetPathsAndLevelsUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new GetPathsAndLevelsModel() { ExpressionId = expressionId }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(
            new ProgressionPathsResponse()
            {
                Paths = results
                    .Value.Select(x => new ProgressionPathReturnModel()
                    {
                        Description = x.Description,
                        Name = x.Name,
                        Id = x.Id,
                        Levels = x
                            .Levels.Select(y => new ProgressionLevelReturnModel()
                            {
                                Description = y.Description,
                                XlLevel = y.XlLevel,
                                Id = y.Id,
                                ModifierGroupId = y.ModifierGroupId,
                            })
                            .ToList(),
                    })
                    .ToList(),
            }
        );
    }
}
