using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.Powers.CreatePower;

public interface ICreatePowerUseCase : IGenericUseCase<Result<int>, CreatePowerModel> { }
