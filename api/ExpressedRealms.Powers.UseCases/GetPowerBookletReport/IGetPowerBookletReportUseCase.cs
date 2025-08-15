using ExpressedRealms.Shared;

namespace ExpressedRealms.Powers.UseCases.GetPowerBookletReport;

public interface IGetPowerBookletReportUseCase
    : IGenericUseCase<MemoryStream, GetPowerBookletReportUseCaseModel> { }
