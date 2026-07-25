using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.GetFactions;

public interface IGetCharacterFactionLevelsUseCase
    : IGenericUseCase<Result<FactionsReturnModel>, GetCharacterFactionLevelsModel> { }
