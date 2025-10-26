using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Delete;

internal sealed class DeleteEventScheduleItemUseCase(
    IEventRepository eventRepository,
    DeleteEventScheduleItemModelValidator validator,
    CancellationToken cancellationToken
) : IDeleteEventScheduleItemUseCase
{
    public async Task<Result> ExecuteAsync(DeleteEventScheduleItemModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var scheduleItem = await eventRepository.GetEventScheduleItem(model.Id);

        scheduleItem!.SoftDelete();

        await eventRepository.EditAsync(scheduleItem!);

        return Result.Ok();
    }
}
