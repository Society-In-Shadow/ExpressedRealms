using ExpressedRealms.Expressions.UseCases.ProgressionPaths.Edit;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Edit;

internal static class EditProgressionPathEndpoint
{
    public static async Task<Results<ValidationProblem, Ok>> ExecuteAsync(
        int expressionId,
        int pathId,
        EditProgressionPath request,
        IEditProgressionPathUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new EditProgressionPathModel()
            {
                Description = request.Description,
                Name = request.Name,
                ExpressionId = expressionId,
                Id = pathId,
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok();
    }
}
