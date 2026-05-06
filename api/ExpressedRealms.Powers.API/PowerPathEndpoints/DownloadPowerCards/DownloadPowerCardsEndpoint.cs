using ExpressedRealms.Powers.UseCases.GetPowerCardReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.DownloadPowerCards;

public static class DownloadPowerCardsEndpoint
{
    public static async Task<FileStreamHttpResult> Execute(
        int expressionId,
        bool isFiveByThree,
        IGetPowerCardReportUseCase useCase
    )
    {
        var reportStream = await useCase.ExecuteAsync(
            new GetPowerCardReportModel()
            {
                ExpressionId = expressionId,
                IsFiveByThree = isFiveByThree,
            }
        );

        return TypedResults.File(
            reportStream,
            contentType: "application/pdf",
            fileDownloadName: "powerCardReport.pdf",
            enableRangeProcessing: true
        );
    }
}
