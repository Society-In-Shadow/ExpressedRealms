using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Events.API.Repositories.Events;

public interface IEventRepository : IGenericRepository
{
    Task<int> CreateEventAsync(Event @event);
    Task BulkAddEventScheduleItems(List<EventScheduleItem> defaultSchedule);
    Task<List<EventScheduleItem>> GetDefaultScheduleItems();
    Task<bool> IsExistingEvent(int id);
    Task<Event> GetEventAsync(int id);
    Task<List<Event>> GetEventsAsync();
    Task<int> CreateEventScheduleItemAsync(EventScheduleItem eventScheduleItem);
    Task<EventScheduleItem?> GetEventScheduleItem(int id);
    Task<List<EventScheduleItem>> GetEventScheduleItems(int eventId);
    Task<Event?> FindEventAsync(int id);
}
