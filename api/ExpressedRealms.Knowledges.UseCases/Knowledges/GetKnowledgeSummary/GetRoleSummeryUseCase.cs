using ExpressedRealms.DB.Shared;
using ExpressedRealms.Knowledges.Repository.Knowledges;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledgeSummary;

internal sealed class GetKnowledgeSummaryUseCase(IKnowledgeRepository repository)
    : IGetKnowledgeSummaryUseCase
{
    public async Task<Result<List<GenericListDto<int>>>> ExecuteAsync()
    {
        return await repository.GetKnowledgeSummaryAsync();
    }
}
