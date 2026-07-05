using ExpressedRealms.Expressions.UseCases.ExpressionUseCases.CopyExpression;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.CopyExpression;

internal static class CopyExpressionEndpoint
{
    public static async Task<Results<ValidationProblem, Ok<int>>> ExecuteAsync(
        int expressionId,
        CopyExpressionRequest request,
        ICopyExpressionUseCase useCase
    )
    {
        var results = await useCase.ExecuteAsync(
            new CopyExpressionModel() { Id = expressionId, ExpressionName = request.Name }
        );

        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(results.Value);
    }
}
