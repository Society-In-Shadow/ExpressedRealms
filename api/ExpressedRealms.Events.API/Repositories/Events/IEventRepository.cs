using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;
using ExpressedRealms.DB.Models.Events.EventSetup;

namespace ExpressedRealms.Events.API.Repositories.Events;

internal interface IEventRepository
{
    Task<int> CreateEventAsync(Event @event);
    Task BulkAddEventScheduleItems(List<EventScheduleItem> defaultSchedule);
    Task<List<EventScheduleItem>> GetDefaultScheduleItems();
    Task<bool> IsExistingEvent(int id);
    Task<Event> GetEventAsync(int id);
    Task UpdateEventAsync(Event @event);
}
