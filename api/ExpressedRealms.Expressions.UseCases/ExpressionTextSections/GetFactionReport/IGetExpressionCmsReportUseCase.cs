using ExpressedRealms.Shared;
using FluentResults;
using QuestPDF.Fluent;

namespace ExpressedRealms.Expressions.UseCases.ExpressionTextSections.GetFactionReport;

public interface IGetFactionReportUseCase
    : IGenericUseCase<Result<MemoryStream>, GetFactionReportModel>
{
    Document? GeneratedReport { get; set; }
    bool GenerateMemoryStream { get; set; }
}
