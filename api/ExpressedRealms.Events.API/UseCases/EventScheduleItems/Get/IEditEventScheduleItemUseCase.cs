using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Get;

public interface IGetEventScheduleItemUseCase
    : IGenericUseCase<Result<EventScheduleItemBaseReturnModel>, GetEventScheduleItemModel> { }
