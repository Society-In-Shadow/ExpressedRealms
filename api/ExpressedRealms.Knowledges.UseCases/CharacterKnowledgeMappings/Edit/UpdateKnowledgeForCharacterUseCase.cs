using ExpressedRealms.Characters.Repository.Xp;
using ExpressedRealms.DB.Characters.xpTables;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using ExpressedRealms.UseCases.Shared.CommonFailureTypes;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;

internal sealed class UpdateKnowledgeForCharacterUseCase(
    ICharacterKnowledgeRepository mappingRepository,
    IKnowledgeLevelRepository knowledgeLevelRepository,
    IKnowledgeRepository knowledgeRepository,
    IXpRepository xpRepository,
    UpdateKnowledgeForCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IUpdateKnowledgeForCharacterUseCase
{
    public async Task<Result> ExecuteAsync(UpdateKnowledgeForCharacterModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var mapping = await mappingRepository.GetCharacterKnowledgeMappingForEditing(
            model.MappingId
        );

        if (mapping.KnowledgeLevelId != model.KnowledgeLevelId)
        {
            var xpInfo = await xpRepository.GetAvailableXpForSection(
                mapping.CharacterId,
                XpSectionTypeEnum.Knowledge
            );

            const int unknownKnowledgeType = 3;

            var newLevel = await knowledgeLevelRepository.GetKnowledgeLevel(model.KnowledgeLevelId);
            var previousLevel = await knowledgeLevelRepository.GetKnowledgeLevel(
                mapping.KnowledgeLevelId
            );

            var knowledge = await knowledgeRepository.GetKnowledgeAsync(mapping.KnowledgeId);
            var spentXp = xpInfo.SpentXp;

            var removedXp =
                knowledge.KnowledgeTypeId == unknownKnowledgeType
                    ? previousLevel.TotalUnknownXpCost
                    : previousLevel.TotalGeneralXpCost;
            spentXp -= removedXp;

            var newExperience =
                knowledge.KnowledgeTypeId == unknownKnowledgeType
                    ? newLevel.TotalUnknownXpCost
                    : newLevel.TotalGeneralXpCost;

            if (spentXp + newExperience > xpInfo.AvailableXp)
                return Result.Fail(
                    new NotEnoughXPFailure(xpInfo.AvailableXp - spentXp, newExperience)
                );

            mapping.KnowledgeLevelId = model.KnowledgeLevelId;
        }

        mapping.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await mappingRepository.UpdateCharacterKnowledgeMapping(mapping);

        return Result.Ok();
    }
}
