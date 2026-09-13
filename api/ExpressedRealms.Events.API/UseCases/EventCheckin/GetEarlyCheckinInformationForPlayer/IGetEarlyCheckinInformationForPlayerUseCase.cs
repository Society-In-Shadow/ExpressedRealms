using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetEarlyCheckinInformationForPlayer;

public interface IGetEarlyCheckinInformationForPlayerUseCase
    : IGenericUseCase<Result<GetEarlyCheckinInformationForPlayerReturnModel>> { }
