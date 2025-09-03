using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.EditBlessingLevel;

public interface IEditBlessingLevelUseCase : IGenericUseCase<Result<int>, EditBlessingLevelModel>
{
}