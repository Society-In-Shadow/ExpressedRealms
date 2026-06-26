using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;

public interface IGetFactionsUseCase
    : IGenericUseCase<Result<FactionsReturnModel>, GetFactionsModel> { }
