using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.Characters.GetBreakOfDawnInfo;

public interface IGetBreakOfDawnInfoUseCase
    : IGenericUseCase<Result<GetBreakOfDawnInfoDto>, GetBreakOfDawnInfoModel> { }
