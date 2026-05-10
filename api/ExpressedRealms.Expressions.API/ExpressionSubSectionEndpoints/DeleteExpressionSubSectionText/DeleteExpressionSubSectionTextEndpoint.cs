using ExpressedRealms.Expressions.Repository.ExpressionTextSections;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DeleteExpressionSubSectionText;

internal static class DeleteExpressionSubSectionTextEndpoint
{
    internal static async Task<Results<NotFound, StatusCodeHttpResult, NoContent>> ExecuteAsync(
        int expressionId,
        int sectionId,
        IExpressionTextSectionRepository repository
    )
    {
        var results = await repository.DeleteExpressionTextSectionAsync(
            expressionId,
            sectionId
        );

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
