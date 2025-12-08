using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events;
using ExpressedRealms.Events.API.UseCases.Events.SendEventPublishedMessages;
using FluentResults;

namespace ExpressedRealms.Events.API.UseCases.Events.TriggerEventReminder;

internal sealed class EventReminderHandlerUseCase(
    IEventRepository eventRepository,
    ISendEventPublishedMessagesUseCase sendEventPublishedMessagesUseCase,
    TimeProvider systemClock
) : IEventReminderHandlerUseCase
{
    public async Task<Result> ExecuteAsync()
    {
        var events = await eventRepository.GetCurrenOrFutureEvents();

        var today = DateOnly.FromDateTime(systemClock.GetUtcNow().DateTime);

        var eventsToPublish = events
            .Select(x => GetEventTypeOrDefault(x, today))
            .Where(x => x.HasValue)
            .ToList();

        foreach (var publishEvents in eventsToPublish)
        {
            await sendEventPublishedMessagesUseCase.ExecuteAsync(
                new SendEventPublishedMessagesModel()
                {
                    Id = publishEvents!.Value.Key,
                    PublishType = publishEvents.Value.Value,
                }
            );
        }

        return Result.Ok();
    }

    private static KeyValuePair<int, PublishType>? GetEventTypeOrDefault(
        Event @event,
        DateOnly today
    )
    {
        if (@event.StartDate == today.AddMonths(1).AddDays(7))
            return new KeyValuePair<int, PublishType>(@event.Id, PublishType.InternalReminder);

        if (@event.StartDate == today.AddDays(7))
            return new KeyValuePair<int, PublishType>(@event.Id, PublishType.OneWeekReminder);

        if (@event.StartDate == today.AddMonths(1))
            return new KeyValuePair<int, PublishType>(@event.Id, PublishType.OneMonthReminder);

        if (@event.StartDate <= today && @event.EndDate >= today)
            return new KeyValuePair<int, PublishType>(@event.Id, PublishType.DayOfReminder);

        return null;
    }
}
