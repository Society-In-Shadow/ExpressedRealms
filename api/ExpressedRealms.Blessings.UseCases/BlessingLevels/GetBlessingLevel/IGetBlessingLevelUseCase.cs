using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;

public interface IGetBlessingLevelUseCase
    : IGenericUseCase<Result<GetBlessingLevelReturnModel>, GetBlessingLevelModel> { }
