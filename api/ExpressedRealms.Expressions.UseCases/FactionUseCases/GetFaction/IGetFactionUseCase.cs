using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFaction;

public interface IGetFactionUseCase
    : IGenericUseCase<Result<GetFactionReturnModel>, GetFactionModel> { }
