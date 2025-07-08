using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.DeleteKnowledge;

internal sealed class DeleteKnowledgeUseCase(
    IKnowledgeRepository knowledgeRepository,
    DeleteKnowledgeModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteKnowledgeUseCase
{
    public async Task<Result> ExecuteAsync(DeleteKnowledgeModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledge = await knowledgeRepository.GetKnowledgeForEditingAsync(model.Id);

        knowledge.SoftDelete();

        await knowledgeRepository.EditKnowledgeAsync(knowledge);

        return Result.Ok();
    }
}
