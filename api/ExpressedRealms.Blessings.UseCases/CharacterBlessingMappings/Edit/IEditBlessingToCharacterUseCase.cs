using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;

public interface IEditBlessingToCharacterUseCase
    : IGenericUseCase<Result<int>, EditBlessingToCharacterModel> { }
