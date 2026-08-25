using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetCharacterStorageOptins;

public interface IGetCharacterStorageOptinsUseCase
    : IGenericUseCase<
        Result<GetCharacterStorageOptinsReturnModel>,
        GetCharacterStorageOptinsModel
    > { }
