using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Models.Characters.XpTables;
using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;

internal sealed class AddKnowledgeToCharacterUseCase(
    ICharacterKnowledgeRepository mappingRepository,
    IKnowledgeLevelRepository knowledgeLevelRepository,
    IKnowledgeRepository knowledgeRepository,
    IXpRepository xpRepository,
    AddKnowledgeToCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IAddKnowledgeToCharacterUseCase
{
    public async Task<Result<int>> ExecuteAsync(AddKnowledgeToCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var xpInfo = await xpRepository.GetAvailableXpForSection(
            model.CharacterId,
            XpSectionTypes.Knowledge
        );

        // Assuming character creation rules for now
        const int unknownKnowledgeType = 3;

        var knowledge = await knowledgeRepository.GetKnowledgeAsync(model.KnowledgeId);
        var knowledgeLevel = await knowledgeLevelRepository.GetKnowledgeLevel(
            model.KnowledgeLevelId
        );
        var newExperience =
            knowledge.KnowledgeTypeId == unknownKnowledgeType
                ? knowledgeLevel.TotalUnknownXpCost
                : knowledgeLevel.TotalGeneralXpCost;

        if (xpInfo.SpentXp + newExperience > xpInfo.AvailableXp)
            return Result.Fail(
                new NotEnoughXPFailure(xpInfo.AvailableXp - xpInfo.SpentXp, newExperience)
            );

        var mappingId = await mappingRepository.AddCharacterKnowledgeMapping(
            new CharacterKnowledgeMapping()
            {
                KnowledgeLevelId = model.KnowledgeLevelId,
                CharacterId = model.CharacterId,
                KnowledgeId = model.KnowledgeId,
                Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim(),
            }
        );

        return Result.Ok(mappingId);
    }
}
