using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.GetCmsExpressionId;

internal static class GetCmsExpressionIdByNameEndpoint
{
    public static async Task<
        Results<NotFound, Ok<ExpressionCmsNameResponse>>
    > GetCmsExpressionIdByName(int id, IExpressionRepository expressionRepository)
    {
        var result = await expressionRepository.GetCmsExpressionId(id);
        if (result.HasNotFound(out var notFound))
            return notFound;
        result.ThrowIfErrorNotHandled();

        return TypedResults.Ok(new ExpressionCmsNameResponse { Id = result.Value });
    }
}
