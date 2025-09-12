using ExpressedRealms.Blessings.Repository.CharacterBlessings;
using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.FeatureFlags;
using ExpressedRealms.FeatureFlags.FeatureClient;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Powers.Repository.CharacterPower;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

internal sealed class GetCharacterExperienceBreakdownUseCase(
    ICharacterKnowledgeRepository mappingRepository,
    ICharacterStatRepository statRepository,
    ICharacterSkillRepository skillRepository,
    ICharacterPowerRepository powerRepository,
    ICharacterBlessingRepository blessingRepository,
    IFeatureToggleClient featureToggleClient,
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

        var knowledgeXp = await mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(
            model.CharacterId
        );
        costs.Add(new ExperienceTotalMax("Knowledge XP", knowledgeXp, 7));

        var powerXp = await powerRepository.GetExperienceSpentOnPowersForCharacter(
            model.CharacterId
        );
        costs.Add(new ExperienceTotalMax("Power XP", powerXp, 20));

        var statsXp = await statRepository.GetExperienceSpentOnStatsForCharacter(model.CharacterId);
        costs.Add(new ExperienceTotalMax("Stat XP", statsXp, 72));

        var skillXp = await skillRepository.GetExperienceSpentOnSkillsForCharacter(
            model.CharacterId
        );

        costs.Add(new ExperienceTotalMax("Skills XP", skillXp, 28));

        costs.Add(new ExperienceTotalMax("Descretionary", -1, 16, false));

        if (await featureToggleClient.HasFeatureFlag(ReleaseFlags.ManageCharacterBlessings))
        {
            var advantageXp = await blessingRepository.GetExperienceSpentOnBlessingsForCharacter(
                model.CharacterId
            );
            costs.Add(new ExperienceTotalMax("Advantage XP", advantageXp, -1, true, false));

            var disadvantageXp = await blessingRepository.GetExperienceAvailableToSpendOnCharacter(
                model.CharacterId
            );
            costs.Add(new ExperienceTotalMax("Disadvantage XP", -1, disadvantageXp, false));
        }

        var totalXp = costs.Where(x => x.IncludeInTotal).Sum(x => x.Total);
        var maxXp = costs.Where(x => x.IncludeInMax).Sum(x => x.Max);

        costs.Add(new ExperienceTotalMax("Total", totalXp, maxXp));

        return Result.Ok(new ExperienceBreakdownReturnModel() { ExperienceSections = costs });
    }
}
