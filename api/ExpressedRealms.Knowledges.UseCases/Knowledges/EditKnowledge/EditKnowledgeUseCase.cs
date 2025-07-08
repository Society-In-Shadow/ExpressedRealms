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

        await knowledgeRepository.EditKnowledgeAsync(
            new Knowledge()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                KnowledgeTypeId = model.KnowledgeTypeId,
            }
        );

        return Result.Ok();
    }
}
