using ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers.ReturnModels;
using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetPickablePowers;

public interface IGetAvailablePowersUseCase
    : IGenericUseCase<Result<List<PowerPathReturnModel>>, GetAvailablePowersModel> { }
