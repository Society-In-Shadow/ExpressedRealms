using ExpressedRealms.Characters.Repository;
using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

internal sealed class GetCharacterExperienceBreakdownUseCase(
    IXpRepository xpRepository,
    ICharacterRepository characterRepository,
    IAssignedXpMappingRepository assignedXpRepository,
    IEventRepository eventRepository,
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
        var characterInfo = await characterRepository.FindCharacterAsync(model.CharacterId);

        var assignedXp = await assignedXpRepository.GetAllPlayerMappingsAsync(
            characterInfo!.PlayerId
        );
        var events = await eventRepository.GetEventsWithAvailableXp();
        var xpAvailable = assignedXp.Sum(x => x.Amount) + events.Sum(x => x.ConExperience);

        costs.AddRange(
            xpInfo
                .Select(x =>
                {
                    // Unlimited XP
                    var max = 1_000_000;

                    if (characterInfo.IsInCharacterCreation)
                    {
                        max = x.SectionCap;
                    }
                    else if (characterInfo.IsPrimaryCharacter)
                    {
                        max = xpAvailable + x.TotalCharacterCreationXp;
                    }

                    return new ExperienceTotalMax(
                        x.SectionName,
                        x.SpentXp,
                        max,
                        x.SectionTypeId,
                        x.LevelXp
                    );
                })
                .ToList()
        );

        return Result.Ok(
            new ExperienceBreakdownReturnModel()
            {
                ExperienceSections = costs,
                AvailableDiscretionary = await xpRepository.GetAvailableDiscretionary(
                    model.CharacterId
                ),
                TotalSpentLevelXp = xpInfo.Sum(x => x.LevelXp),
                TotalAvailableXp = xpAvailable,
            }
        );
    }
}
