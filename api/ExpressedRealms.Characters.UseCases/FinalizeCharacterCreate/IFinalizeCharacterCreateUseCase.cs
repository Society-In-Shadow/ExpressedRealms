using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.FinalizeCharacterCreate;

public interface IFinalizeCharacterCreateUseCase
    : IGenericUseCase<Result, FinalizeCharacterCreateModel> { }
