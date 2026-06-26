using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.CreateFaction;

public interface ICreateFactionUseCase : IGenericUseCase<Result<int>, CreateFactionModel> { }
