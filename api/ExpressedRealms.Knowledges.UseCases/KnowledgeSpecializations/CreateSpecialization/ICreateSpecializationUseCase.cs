using ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeSpecializations.CreateSpecialization;

public interface ICreateSpecializationUseCase
    : IGenericUseCase<Result<int>, CreateSpecializationModel> { }
