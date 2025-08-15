using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionCmsReport;
using ExpressedRealms.Server.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Expressions.API.ExpressionEndpoints.GetExpressionCmsReport;

internal static class GetExpressionCmsReportEndpoint
{
    internal static async Task<Results<NotFound, FileStreamHttpResult, StatusCodeHttpResult>> GetExpressionCmsReport(
        int id,
        IGetExpressionCmsReportUseCase repository
    )
    {
        var status = await repository.ExecuteAsync(new GetExpressionCmsReportModel()
        {
            ExpressionId = id
        });

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
        );;
    }
}
