using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Events.API.Repositories.Events.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Events.API.Repositories.Events;

internal sealed class EventRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IEventRepository
{
    public async Task<int> CreateEventAsync(Event @event)
    {
        context.Events.Add(@event);
        await context.SaveChangesAsync(cancellationToken);
        return @event.Id;
    }

    public async Task<List<EventScheduleItem>> GetDefaultScheduleItems()
    {
        return await context
            .EventScheduleItems.AsNoTracking()
            .Where(x => x.EventId == 1)
            .ToListAsync(cancellationToken);
    }

    public async Task BulkAddEventScheduleItems(List<EventScheduleItem> defaultSchedule)
    {
        context.EventScheduleItems.AddRange(defaultSchedule);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsExistingEvent(int id)
    {
        return await context.Events.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Event> GetEventAsync(int id)
    {
        return context.Events.FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Event?> FindEventAsync(int id)
    {
        return await context.Events.FindAsync(id, cancellationToken);
    }

    public async Task<List<EventXpDto>> GetEventsWithAvailableXp()
    {
        var availableDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1));
        return await context
            .Events.AsNoTracking()
            .Where(x => x.StartDate <= availableDate && x.IsPublished)
            .Select(x => new EventXpDto()
            {
                Name = x.Name,
                StartDate = x.StartDate,
                ConExperience = x.ConExperience,
            })
            .ToListAsync(cancellationToken);
    }

    public Task<List<Event>> GetEventsAsync()
    {
        return context.Events.ToListAsync(cancellationToken);
    }

    public async Task<int> CreateEventScheduleItemAsync(EventScheduleItem eventScheduleItem)
    {
        context.EventScheduleItems.Add(eventScheduleItem);
        await context.SaveChangesAsync(cancellationToken);
        return eventScheduleItem.Id;
    }

    public async Task<EventScheduleItem?> GetEventScheduleItem(int id)
    {
        return await context.EventScheduleItems.FindAsync(id);
    }

    public Task<List<EventScheduleItem>> GetEventScheduleItems(int eventId)
    {
        return context
            .EventScheduleItems.AsNoTracking()
            .Where(x => x.EventId == eventId)
            .ToListAsync(cancellationToken);
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
