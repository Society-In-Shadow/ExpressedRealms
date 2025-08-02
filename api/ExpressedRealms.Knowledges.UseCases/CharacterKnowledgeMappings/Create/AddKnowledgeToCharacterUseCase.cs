using ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.Repository;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMapping;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.Repositories.Shared.CommonFailureTypes;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;

internal sealed class AddKnowledgeToCharacterUseCase(
    CharacterKnowledgeRepository mappingRepository,
    IKnowledgeLevelRepository knowledgeLevelRepository,
    IKnowledgeRepository knowledgeRepository,
    AddModelValidator validator,
    CancellationToken cancellationToken)
{
    public async Task<Result<int>> Execute(AddModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);
        
        // Get Character
        // Check if it's an active character
        // If so, check create status
        
        const int unknownKnowledgeType = 3;
        const int availableExperience = 7;
        
        var spentXp = await mappingRepository.GetExperienceSpentOnKnowledgesForCharacter(model.CharacterId);
        var knowledge = await knowledgeRepository.GetKnowledgeAsync(model.KnowledgeId);
        var knowledgeLevel = await knowledgeLevelRepository.GetKnowledgeLevel(model.KnowledgeLevelId);
        var newExperience =
            knowledge.KnowledgeTypeId == unknownKnowledgeType ? knowledgeLevel.UnknownXpCost : knowledgeLevel.GeneralXpCost;
        
        if(spentXp + newExperience > availableExperience)
            return Result.Fail(new NotEnoughXPFailure(availableExperience, newExperience));
        
        var mappingId = await mappingRepository.AddCharacterKnowledgeMapping(new CharacterKnowledgeMapping()
        {
            KnowledgeLevelId = model.KnowledgeLevelId,
            CharacterId = model.CharacterId,
            KnowledgeId = model.KnowledgeId,
            Notes = model.Notes?.Trim()
        });

        return Result.Ok(mappingId);
    }
}