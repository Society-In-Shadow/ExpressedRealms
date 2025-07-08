using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;

internal sealed class CreateKnowledgeUseCase(
    IKnowledgeRepository knowledgeRepository,
    CreateKnowledgeModelValidator validator,
    CancellationToken cancellationToken
) : ICreateKnowledgeUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateKnowledgeModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledgeId = await knowledgeRepository.CreateKnowledgeAsync(
            new Knowledge()
            {
                Name = model.Name,
                Description = model.Description,
                KnowledgeTypeId = model.KnowledgeTypeId,
            }
        );

        return Result.Ok(knowledgeId);
    }
}
