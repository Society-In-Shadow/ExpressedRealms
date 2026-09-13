using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;
using ExpressedRealms.DB.Models.Events.EventSetup;

namespace ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;

[AuditInclude]
public class EventScheduleItem : ISoftDelete
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual List<EventScheduleItemAuditTrail> EventScheduleItemAuditTrails { get; set; } =
        null!;
}
