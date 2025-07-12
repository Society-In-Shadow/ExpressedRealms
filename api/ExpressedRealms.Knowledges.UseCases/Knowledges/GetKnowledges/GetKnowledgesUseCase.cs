using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledges;

internal sealed class GetKnowledgesUseCase(IKnowledgeRepository knowledgeRepository)
    : IGetKnowledgesUseCase
{
    public async Task<Result<KnowledgeReturnModel>> ExecuteAsync()
    {
        var knowledge = await knowledgeRepository.GetKnowledges();

        return Result.Ok(
            new KnowledgeReturnModel()
            {
                KnowledgeTypes = knowledge
                    .Select(x => new KnowledgeModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        TypeName = x.KnowledgeType.Name,
                        TypeDescription = x.KnowledgeType.Description,
                        TypeId = x.KnowledgeTypeId,
                    })
                    .ToList(),
            }
        );
    }
}
