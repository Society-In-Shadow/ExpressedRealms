using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Edit;

internal sealed class EditEventScheduleItemUseCase(
    IEventRepository eventRepository,
    EditEventScheduleItemModelValidator validator,
    CancellationToken cancellationToken
) : IEditEventScheduleItemUseCase
{
    public async Task<Result> ExecuteAsync(EditEventScheduleItemModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var scheduleItem = await eventRepository.GetEventScheduleItem(model.Id);

        scheduleItem!.Description = model.Description;
        scheduleItem.Date = model.Date;
        scheduleItem.StartTime = model.StartTime;
        scheduleItem.EndTime = model.EndTime;
        
        await eventRepository.EditAsync(scheduleItem);

        return Result.Ok();
    }
}
