using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Characters.UseCases.AssignedXp.Get;

public interface IGetAssignedXpMappingUseCase
    : IGenericUseCase<Result<List<XpMappingInfoReturnModel>>, GetAssignedXpMappingModel> { }
