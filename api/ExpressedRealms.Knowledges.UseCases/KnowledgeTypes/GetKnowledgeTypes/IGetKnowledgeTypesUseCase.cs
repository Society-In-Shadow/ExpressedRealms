using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeTypes.GetKnowledgeTypes;

public interface IGetKnowledgeTypesUseCase
    : IGenericUseCase<Result<GetKnowledgeTypeReturnModel>> { }
