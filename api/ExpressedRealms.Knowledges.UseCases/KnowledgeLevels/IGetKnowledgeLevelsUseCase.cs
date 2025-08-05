using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;

public interface IGetKnowledgeLevelsUseCase : IGenericUseCase<Result<GetKnowledgeLevelsReturnModel>, GetKnowledgeLevelsModel> { }
