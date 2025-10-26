using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.UseCases.Shared;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Create;

internal sealed class CreateEventScheduleItemUseCase(
    IEventRepository eventRepository,
    CreateEventScheduleItemModelValidator validator,
    CancellationToken cancellationToken
) : ICreateEventScheduleItemUseCase
{
    public async Task<Result<int>> ExecuteAsync(CreateEventScheduleItemModel model)
    {
        var result = await ValidationHelper.ValidateAndHandleErrorsAsync(
            validator,
            model,
            cancellationToken
        );

        if (result.IsFailed)
            return Result.Fail(result.Errors);

        var eventId = await eventRepository.CreateEventScheduleItemAsync(
            new EventScheduleItem()
            {
                EventId = model.EventId,
                Description = model.Description,
                Date = model.Date,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
            }
        );

        return Result.Ok(eventId);
    }
}
