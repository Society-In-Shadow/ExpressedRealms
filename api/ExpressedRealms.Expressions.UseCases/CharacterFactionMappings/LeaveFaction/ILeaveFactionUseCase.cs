using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.LeaveFaction;

public interface ILeaveFactionUseCase : IGenericUseCase<Result<int>, LeaveFactionModel> { }
