using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.EditKnowledge;

internal sealed class EditKnowledgeUseCase(
    IKnowledgeRepository knowledgeRepository,
    EditKnowledgeModelValidator validator,
    CancellationToken cancellationToken
) : IEditKnowledgeUseCase
{
    public async Task<Result> ExecuteAsync(EditKnowledgeModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledge = await knowledgeRepository.GetKnowledgeForEditingAsync(model.Id);

        knowledge.Name = model.Name;
        knowledge.Description = model.Description;
        knowledge.KnowledgeTypeId = model.KnowledgeTypeId;

        await knowledgeRepository.EditKnowledgeAsync(knowledge);

        return Result.Ok();
    }
}
