using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionBooklet;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.GetExpressionBooklet;

internal static class GetExpressionBookletEndpoint
{
    internal static async Task<
        Results<NotFound, FileStreamHttpResult, StatusCodeHttpResult, ValidationProblem>
    > GetExpressionBooklet(int expressionId, IGetExpressionBookletUseCase repository)
    {
        var status = await repository.ExecuteAsync(
            new GetExpressionBookletModel() { ExpressionId = expressionId }
        );

        if (status.HasValidationError(out var validation))
            return validation;
        if (status.HasNotFound(out var notFound))
            return notFound;
        if (status.HasBeenDeletedAlready(out var deletedAlready))
            return deletedAlready;
        status.ThrowIfErrorNotHandled();

        return TypedResults.File(
            status.Value,
            contentType: "application/pdf",
            fileDownloadName: "powerBookletReport.pdf",
            enableRangeProcessing: true
        );
        ;
    }
}
