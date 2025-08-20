using ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers.ReturnModels;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPowers;

public interface IGetCharacterPowersUseCase
    : IGenericUseCase<Result<List<PowerPathReturnModel>>, GetCharacterPowersModel> { }
