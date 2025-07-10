using ExpressedRealms.Knowledges.Repository.Knowledges;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetEditKnowledge;

internal sealed class GetEditKnowledgeUseCase(
    IKnowledgeRepository knowledgeRepository,
    GetEditKnowledgeModelValidator validator,
    CancellationToken cancellationToken
) : IGetEditKnowledgeUseCase
{
    public async Task<Result<GetEditKnowledgeReturnModel>> ExecuteAsync(GetEditKnowledgeModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var knowledge = await knowledgeRepository.GetKnowledgeAsync(model.Id);

        return Result.Ok(
            new GetEditKnowledgeReturnModel()
            {
                Id = knowledge.Id,
                Description = knowledge.Description,
                Name = knowledge.Name,
                KnowledgeTypeId = knowledge.KnowledgeTypeId,
            }
        );
    }
}
