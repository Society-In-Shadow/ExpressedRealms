using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeTypes.GetKnowledgeTypes;

public class GetKnowledgeTypesUseCase(IKnowledgeRepository knowledgeRepository)
    : IGetKnowledgeTypesUseCase
{
    public async Task<Result<GetKnowledgeTypeReturnModel>> ExecuteAsync()
    {
        var knowledgeTypes = await knowledgeRepository.GetKnowledgeTypesAsync();

        return Result.Ok(
            new GetKnowledgeTypeReturnModel()
            {
                KnowledgeTypes = knowledgeTypes
                    .Select(x => new KnowledgeTypeModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    })
                    .ToList(),
            }
        );
    }
}
