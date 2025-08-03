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

            var knowledgeLevel = await knowledgeLevelRepository.GetKnowledgeLevel(
                model.KnowledgeLevelId
            );

            var knowledge = await knowledgeRepository.GetKnowledgeAsync(mapping.KnowledgeId);
            var newExperience =
                knowledge.KnowledgeTypeId == unknownKnowledgeType
                    ? knowledgeLevel.UnknownXpCost
                    : knowledgeLevel.GeneralXpCost;

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
