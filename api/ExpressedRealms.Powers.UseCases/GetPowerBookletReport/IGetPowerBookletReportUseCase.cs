using ExpressedRealms.Shared;
using FluentResults;
using QuestPDF.Fluent;

namespace ExpressedRealms.Powers.UseCases.GetPowerBookletReport;

public interface IGetPowerBookletReportUseCase
    : IGenericUseCase<Result<MemoryStream>, GetPowerBookletReportModel>
{
    public Document? GeneratedReport { get; set; }
    public bool GenerateMemoryStream { get; set; }
}
