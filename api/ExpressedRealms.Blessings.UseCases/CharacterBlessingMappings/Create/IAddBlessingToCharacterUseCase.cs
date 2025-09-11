using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;

public interface IAddBlessingToCharacterUseCase
    : IGenericUseCase<Result<int>, AddBlessingToCharacterModel> { }
