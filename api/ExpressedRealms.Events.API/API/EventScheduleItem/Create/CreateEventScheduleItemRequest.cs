namespace ExpressedRealms.Events.API.API.EventScheduleItem.Create;

public class CreateEventScheduleItemRequest
{
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
}
