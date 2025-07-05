using ExpressedRealms.Expressions.Repository.Expressions;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.DeleteExpression;

internal static class DeleteExpressionEndpoint
{
    internal static async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> DeleteExpression(
        int id,
        IExpressionRepository repository
    )
    {
        var status = await repository.DeleteExpressionAsync(id);

        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        status.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
