namespace ExpressedRealms.Events.API.API.EventScheduleItem.Create;

public class CreateEventScheduleItemRequest
{
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
}
