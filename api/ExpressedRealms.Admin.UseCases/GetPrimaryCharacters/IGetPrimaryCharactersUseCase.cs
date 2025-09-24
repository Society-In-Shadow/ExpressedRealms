using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Admin.UseCases.GetPrimaryCharacters;

public interface IGetPrimaryCharactersUseCase : IGenericUseCase<Result<List<PrimaryCharacterReturnInfo>>>
{
}