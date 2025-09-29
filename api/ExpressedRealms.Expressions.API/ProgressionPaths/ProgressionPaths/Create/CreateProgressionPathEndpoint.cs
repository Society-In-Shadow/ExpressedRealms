using ExpressedRealms.Expressions.UseCases.ProgressionPaths.Add;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ProgressionPaths.ProgressionPaths.Create;

internal static class CreateProgressionPathEndpoint
{
    public static async Task<Results<ValidationProblem, Created<int>>> ExecuteAsync(
        int expressionId,
        CreateProgressionPath request,
        IAddProgressionPathUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new AddProgressionPathModel()
            {
                Description = request.Description,
                Name = request.Name,
                ExpressionId = expressionId
            }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Created("/expressions", results.Value);
    }
}
