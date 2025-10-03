using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;

public interface IGetCharacterSheetReportUseCase
    : IGenericUseCase<Result<MemoryStream>, GetCharacterSheetReportModel> { }
