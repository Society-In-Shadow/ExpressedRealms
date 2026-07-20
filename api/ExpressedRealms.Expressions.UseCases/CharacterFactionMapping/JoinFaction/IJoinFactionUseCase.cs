using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.JoinFaction;

public interface IJoinFactionUseCase : IGenericUseCase<Result<int>, JoinFactionModel> { }
