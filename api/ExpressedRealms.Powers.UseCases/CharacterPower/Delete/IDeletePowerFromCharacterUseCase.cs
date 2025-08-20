using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.Delete;

public interface IDeletePowerFromCharacterUseCase
    : IGenericUseCase<Result, DeletePowerFromCharacterModel> { }
