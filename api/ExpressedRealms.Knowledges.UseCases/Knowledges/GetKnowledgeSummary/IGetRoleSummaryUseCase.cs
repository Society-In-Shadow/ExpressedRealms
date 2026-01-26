using ExpressedRealms.DB.Shared;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledgeSummary;

public interface IGetKnowledgeSummaryUseCase
    : IGenericUseCase<Result<List<GenericListDto<int>>>> { }
