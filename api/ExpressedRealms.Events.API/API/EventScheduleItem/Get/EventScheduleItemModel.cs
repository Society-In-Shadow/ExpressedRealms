namespace ExpressedRealms.Events.API.API.EventScheduleItem.Get;

public class EventScheduleItemModel
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
}
