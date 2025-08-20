using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Create;

public interface IAddPowerToCharacterUseCase
    : IGenericUseCase<Result<int>, AddPowerToCharacterModel> { }
