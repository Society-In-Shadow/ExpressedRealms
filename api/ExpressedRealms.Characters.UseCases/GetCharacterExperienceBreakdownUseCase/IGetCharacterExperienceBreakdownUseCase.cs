using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.GetCharacterExperienceBreakdownUseCase;

public interface IGetCharacterExperienceBreakdownUseCase
    : IGenericUseCase<Result<ExperienceBreakdownReturnModel>, GetCharacterExperienceBreakdownModel> { }
