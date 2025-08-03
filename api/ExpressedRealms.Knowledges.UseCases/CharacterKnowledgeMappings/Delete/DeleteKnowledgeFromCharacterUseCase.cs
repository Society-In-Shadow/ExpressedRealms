using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings;
using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Delete;

internal sealed class DeleteKnowledgeFromCharacterUseCase(
    ICharacterKnowledgeRepository mappingRepository,
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

        mapping.SoftDelete();

        await mappingRepository.UpdateCharacterKnowledgeMapping(mapping);

        return Result.Ok();
    }
}
