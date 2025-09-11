using ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Delete;

public interface IDeleteBlessingFromCharacterUseCase
    : IGenericUseCase<Result, UpdateBlessingForCharacterModel> { }
