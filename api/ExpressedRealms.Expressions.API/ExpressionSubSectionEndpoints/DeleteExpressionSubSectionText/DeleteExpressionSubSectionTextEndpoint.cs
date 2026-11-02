using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.DeleteTextSection;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionSubSectionEndpoints.DeleteExpressionSubSectionText;

internal static class DeleteExpressionSubSectionTextEndpoint
{
    internal static async Task<Results<NotFound, StatusCodeHttpResult, NoContent>> ExecuteAsync(
        int expressionId,
        int sectionId,
        IDeleteTextSectionUseCase repository
    )
    {
        var results = await repository.ExecuteAsync(new DeleteTextSectionModel()
        {
            Id = sectionId,
            ExpressionId = expressionId
        });

        if (results.HasNotFound(out var notFound))
            return notFound;
        if (results.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        results.ThrowIfErrorNotHandled();

        return TypedResults.NoContent();
    }
}
