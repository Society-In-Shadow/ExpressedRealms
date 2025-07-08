using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetEditKnowledge;

public interface IGetEditKnowledgeUseCase
    : IGenericUseCase<Result<GetEditKnowledgeReturnModel>, GetEditKnowledgeModel> { }
