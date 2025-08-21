using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Powers.UseCases.CharacterPower.GetOptions;

public interface IGetCharacterPowerOptionsUseCase
    : IGenericUseCase<
        Result<GetCharacterPowerOptionsReturnModel>,
        GetCharacterPowerOptionsModel
    > { }
