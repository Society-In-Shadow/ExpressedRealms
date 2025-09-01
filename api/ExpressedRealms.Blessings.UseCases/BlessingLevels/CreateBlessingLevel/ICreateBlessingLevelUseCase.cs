using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.CreateBlessingLevel;

public interface ICreateBlessingLevelUseCase : IGenericUseCase<Result<int>, CreateBlessingLevelModel>
{
}