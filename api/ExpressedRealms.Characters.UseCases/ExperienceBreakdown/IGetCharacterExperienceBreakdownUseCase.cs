using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

public interface IGetCharacterExperienceBreakdownUseCase
    : IGenericUseCase<
        Result<ExperienceBreakdownReturnModel>,
        GetCharacterExperienceBreakdownModel
    > { }
