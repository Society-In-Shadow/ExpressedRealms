using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.GetFactions;

public interface IGetCharacterFactionLevelsUseCase
    : IGenericUseCase<Result<FactionsReturnModel>, GetCharacterFactionLevelsModel> { }
