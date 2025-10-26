using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Create;

public interface ICreateEventScheduleItemUseCase
    : IGenericUseCase<Result<int>, CreateEventScheduleItemModel> { }
