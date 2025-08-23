using ExpressedRealms.Characters.Repository.Skills;
using ExpressedRealms.Characters.Repository.Stats;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.GetCharacterExperienceBreakdownUseCase;

internal sealed class GetCharacterExperienceBreakdownUseCase(
    ICharacterKnowledgeRepository mappingRepository,
    ICharacterStatRepository statRepository,
    ICharacterSkillRepository skillRepository,
    GetCharacterExperienceBreakdownModelValidator validator,
    CancellationToken cancellationToken
) : IGetCharacterExperienceBreakdownUseCase
{
    public async Task<Result<ExperienceBreakdownReturnModel>> ExecuteAsync(GetCharacterExperienceBreakdownModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledgeXp = await mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(model.CharacterId);
        var powerXp = await mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(model.CharacterId);
        var statsXp = await statRepository.GetExperienceSpentOnStatsForCharacter(model.CharacterId);
        var skillXp = await skillRepository.GetExperienceSpentOnSkillsForCharacter(model.CharacterId);
        
        return Result.Ok(new ExperienceBreakdownReturnModel()
        {
            KnowledgeXp = knowledgeXp,
            SetupKnowledgeXp = 7,
            PowersXp = powerXp,
            SetupPowersXp = 20,
            StatsXp = statsXp,
            SetupStatsXp = 72,
            SkillsXp = skillXp,
            SetupSkillsXp = 28,
            Total = knowledgeXp + powerXp + statsXp + skillXp,
        });
    }
}
