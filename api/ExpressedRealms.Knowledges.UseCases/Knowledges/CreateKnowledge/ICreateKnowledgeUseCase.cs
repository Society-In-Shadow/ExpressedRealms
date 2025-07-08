using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;

public interface ICreateKnowledgeUseCase : IGenericUseCase<Result<int>, CreateKnowledgeModel> { }
