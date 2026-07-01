using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.Expressions.Repository.FactionLevels;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionLevelUseCases.CreateFactionLevel;

internal sealed class CreateFactionLevelUseCase(
    IFactionLevelRepository factionLevelRepository,
    IKnowledgeRepository knowledgeRepository,
    CreateFactionLevelModelValidator validator,
    CancellationToken cancellationToken
) : ICreateFactionLevelUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateFactionLevelModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        // Faction Check skipped, as this should only be called from withing the Create Faction Use Case

        var knowledgeExists = await knowledgeRepository.IsExistingKnowledge(model.KnowledgeId);
        if (!knowledgeExists)
            return Result.Fail(new NotFoundFailure(nameof(CreateFactionLevelModel.KnowledgeId), "Knowledge not found"));
        
        var factionLevels = new List<FactionLevel>()
        {
            new FactionLevel()
            {
                FactionId = model.FactionId,
                FactionRankId = FactionRankEnum.Basic.Value
            },
            new FactionLevel()
            {
                FactionId = model.FactionId,
                FactionRankId = FactionRankEnum.Intermediate.Value,
                KnowledgeId = model.KnowledgeId,
                KnowledgeLevelId = 3, // Student, level 2 knowledge
                Specialization = model.Specialization,
            },
            new FactionLevel()
            {
                FactionId = model.FactionId,
                FactionRankId = FactionRankEnum.Advance.Value,
                KnowledgeId = model.KnowledgeId,
                KnowledgeLevelId = 5, // Associates, level 4 knowledge
                Specialization = model.Specialization,
            },
            new FactionLevel()
            {
                FactionId = model.FactionId,
                FactionRankId = FactionRankEnum.Supreme.Value,
                KnowledgeId = model.KnowledgeId,
                KnowledgeLevelId = 7, // Master's, level 6 knowledge
                Specialization = model.Specialization,
            }
        };
        
        await factionLevelRepository.CreateFactionLevelsAsync(factionLevels);

        return Result.Ok();
    }
}
