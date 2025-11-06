using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Create;

public interface ICreateAssignedXpMappingUseCase
    : IGenericUseCase<Result<int>, CreateAssignedXpMappingModel> { }
