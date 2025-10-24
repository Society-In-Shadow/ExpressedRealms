using Audit.EntityFramework;
using ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;

namespace ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;

[AuditInclude]
public class EventScheduleItem
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }

    public virtual List<EventScheduleItemAuditTrail> EventScheduleItemAuditTrails { get; set; } = null!;
}
