using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.JoinFaction;

public interface IJoinFactionUseCase : IGenericUseCase<Result<int>, JoinFactionModel> { }
