using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
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

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
