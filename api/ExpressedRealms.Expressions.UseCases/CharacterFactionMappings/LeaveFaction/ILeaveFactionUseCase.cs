using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.LeaveFaction;

public interface ILeaveFactionUseCase : IGenericUseCase<Result<int>, LeaveFactionModel> { }
