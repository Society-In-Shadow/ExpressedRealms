using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventCheckin.PlayerRequestsPreCheckin;

public interface IPlayerRequestsPreCheckinUseCase
    : IGenericUseCase<Result, PlayerRequestsPreCheckinModel> { }
