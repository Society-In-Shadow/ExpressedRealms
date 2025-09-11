using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Get;

public interface IGetAssignedBlessingsUseCase : IGenericUseCase<Result<List<CharacterBlessingReturnModel>>, GetAssignedBlessingsModel>
{
}