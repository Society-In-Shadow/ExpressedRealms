using ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionCmsReport;
using ExpressedRealms.Powers.UseCases.GetPowerBookletReport;
using ExpressedRealms.UseCases.Shared;
using FluentResults;
using JetBrains.Annotations;
using QuestPDF.Fluent;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionBooklet;

[UsedImplicitly]
internal sealed class GetExpressionBookletUseCase(
    IGetExpressionCmsReportUseCase backgroundReport,
    IGetPowerBookletReportUseCase powerReport,
    GetExpressionBookletModelValidator validator,
    CancellationToken cancellationToken
) : IGetExpressionBookletUseCase
{
    public async Task<Result<MemoryStream>> ExecuteAsync(GetExpressionBookletModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        backgroundReport.GenerateMemoryStream = false;
        await backgroundReport.ExecuteAsync(new GetExpressionCmsReportModel() { ExpressionId = model.ExpressionId });
        
        powerReport.GenerateMemoryStream = false;
        await powerReport.ExecuteAsync(new GetPowerBookletReportUseCaseModel() { ExpressionId = model.ExpressionId });
        
        var report = Document.Merge(
            backgroundReport.GeneratedReport, 
            powerReport.GeneratedReport
        );

        report.UseContinuousPageNumbers();
        
        var stream = new MemoryStream();
        report.GeneratePdf(stream);

        stream.Position = 0;
        return stream;
    }
}
