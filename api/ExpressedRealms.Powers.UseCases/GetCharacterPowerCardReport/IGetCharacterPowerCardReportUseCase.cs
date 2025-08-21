using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.GetCharacterPowerCardReport;

public interface IGetCharacterPowerCardReportUseCase
    : IGenericUseCase<Result<MemoryStream>, GetCharacterPowerCardReportModel> { }
