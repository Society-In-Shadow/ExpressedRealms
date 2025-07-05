using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.GetEditExpression;

internal static class GetEditExpressionEndpoint
{
    internal static async Task<
        Results<NotFound, ValidationProblem, Ok<EditExpressionResponse>>
    > GetEditExpression(int expressionId, IExpressionRepository repository)
    {
        var results = await repository.GetExpression(expressionId);

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasValidationError(out var validationProblem))
            return validationProblem;
        results.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new EditExpressionResponse(results.Value));
    }
}
