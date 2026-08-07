using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Expressions.Repository.CharacterFactions;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;

internal sealed class DeleteKnowledgeFromCharacterUseCase(
    ICharacterKnowledgeRepository mappingRepository,
    ICharacterFactionRepository characterFactionRepository,
    DeleteKnowledgeFromCharacterModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteKnowledgeFromCharacterUseCase
{
    public async Task<Result> ExecuteAsync(DeleteKnowledgeFromCharacterModel model)
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
        
        var factionKnowledge = await characterFactionRepository.GetLatestPlayerFactionLevels(model.CharacterId);

        if (factionKnowledge.Any(x => x.KnowledgeId == mapping.KnowledgeId))
        {
            return ValidationHelper.AddSingleValidationFailure(nameof(model.MappingId),
                "Your faction level prevents you from removing this knowledge");
        }

        mapping.SoftDelete();

        await mappingRepository.UpdateCharacterKnowledgeMapping(mapping);

        return Result.Ok();
    }
}
