using ExpressedRealms.Characters.Reports.CRB.Data;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Reports.GetCharacterBooklet;

public interface IGetCharacterSheetDataUseCase
    : IGenericUseCase<Result<ReportData>, GetCharacterSheetDataModel> { }
