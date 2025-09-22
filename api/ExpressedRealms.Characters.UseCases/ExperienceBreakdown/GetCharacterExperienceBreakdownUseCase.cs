using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

internal sealed class GetCharacterExperienceBreakdownUseCase(
    IXpRepository xpRepository,
    ICharacterRepository characterRepository,
    GetCharacterExperienceBreakdownModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterExperienceBreakdownUseCase
{
    public async Task<Result<ExperienceBreakdownReturnModel>> ExecuteAsync(
        GetCharacterExperienceBreakdownModel model
    )
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var costs = new List<ExperienceTotalMax>();

        var xpInfo = await xpRepository.GetCharacterXpMappings(model.CharacterId);
        var characterState = await characterRepository.GetCharacterState(model.CharacterId);
        
        costs.AddRange(
            xpInfo
                .Select(x => new ExperienceTotalMax(
                    x.SectionName,
                    x.SpentXp,
                    x.SectionCap,
                    x.SectionTypeId,
                    x.LevelXp
                ))
                .ToList()
        );

        return Result.Ok(
            new ExperienceBreakdownReturnModel()
            {
                ExperienceSections = costs,
                AvailableDiscretionary = await xpRepository.GetAvailableDiscretionary(
                    model.CharacterId
                ),
            }
        );
    }
}
