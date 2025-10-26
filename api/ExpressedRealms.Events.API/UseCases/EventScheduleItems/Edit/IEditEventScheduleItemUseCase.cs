using ExpressedRealms.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Edit;

public interface IEditEventScheduleItemUseCase
    : IGenericUseCase<Result, EditEventScheduleItemModel> { }
