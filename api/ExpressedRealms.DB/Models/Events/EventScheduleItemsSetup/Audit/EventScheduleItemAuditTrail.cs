using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Events.EventSetup;
using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserSetup;

namespace ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;

public class EventScheduleItemAuditTrail : IAuditTable
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    public int EventScheduleItemId { get; set; }
    public virtual EventScheduleItem EventScheduleItem { get; set; } = null!;

    public int Id { get; set; }
    public required string Action { get; set; }
    public DateTime Timestamp { get; set; }
    public required string ActorUserId { get; set; }
    public required string ChangedProperties { get; set; }
    public virtual User ActorUser { get; set; } = null!;
}
