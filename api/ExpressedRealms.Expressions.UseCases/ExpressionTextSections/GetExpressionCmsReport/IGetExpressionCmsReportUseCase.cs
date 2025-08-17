using ExpressedRealms.Shared;
using FluentResults;
using QuestPDF.Fluent;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetExpressionCmsReport;

public interface IGetExpressionCmsReportUseCase
    : IGenericUseCase<Result<MemoryStream>, GetExpressionCmsReportModel>
{
    Document? GeneratedReport { get; set; }
    bool GenerateMemoryStream { get; set; }
}
