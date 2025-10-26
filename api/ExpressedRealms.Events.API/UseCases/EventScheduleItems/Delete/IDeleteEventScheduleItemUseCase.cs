using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Delete;

public interface IDeleteEventScheduleItemUseCase
    : IGenericUseCase<Result, DeleteEventScheduleItemModel> { }
