using ExpressedRealms.Powers.UseCases.GetPowerBookletReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.DownloadPowerBooklet;

public static class DownloadPowerBookletEndpoint
{
    public static async Task<FileStreamHttpResult> Execute(
        int expressionId,
        IGetPowerBookletReportUseCase useCase
    )
    {
        var reportStream = await useCase.ExecuteAsync(
            new GetPowerBookletReportModel() { ExpressionId = expressionId }
        );

        return TypedResults.File(
            reportStream.Value,
            contentType: "application/pdf",
            fileDownloadName: "powerBookletReport.pdf",
            enableRangeProcessing: true
        );
    }
}
