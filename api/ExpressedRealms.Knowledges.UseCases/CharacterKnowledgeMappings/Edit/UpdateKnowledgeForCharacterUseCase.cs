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
            // Get Character
            // Check if it's an active character
            // If so, check create status

            // Also need to take into consideration discretionary spending

            const int unknownKnowledgeType = 3;
            const int availableExperience = 7;

            var spentXp = await mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(
                mapping.CharacterId
            );

            var newLevel = await knowledgeLevelRepository.GetKnowledgeLevel(model.KnowledgeLevelId);

            var previousLevel = await knowledgeLevelRepository.GetKnowledgeLevel(
                mapping.KnowledgeLevelId
            );

            var knowledge = await knowledgeRepository.GetKnowledgeAsync(mapping.KnowledgeId);

            if (newLevel.Level < previousLevel.Level)
            {
                var removedXp =
                    knowledge.KnowledgeTypeId == unknownKnowledgeType
                        ? previousLevel.UnknownXpCost
                        : previousLevel.GeneralXpCost;
                spentXp -= removedXp;
            }

            var newExperience =
                knowledge.KnowledgeTypeId == unknownKnowledgeType
                    ? newLevel.UnknownXpCost
                    : newLevel.GeneralXpCost;

            if (spentXp + newExperience > availableExperience)
                return Result.Fail(
                    new NotEnoughXPFailure(availableExperience - spentXp, newExperience)
                );

            mapping.KnowledgeLevelId = model.KnowledgeLevelId;
        }

        mapping.Notes = model.Notes?.Trim() == string.Empty ? null : model.Notes?.Trim();

        await mappingRepository.UpdateCharacterKnowledgeMapping(mapping);

        return Result.Ok();
    }
}
