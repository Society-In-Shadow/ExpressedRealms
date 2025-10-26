namespace ExpressedRealms.Events.API.API.EventScheduleItem.Edit;

public class EditEventScheduleItemRequest
{
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
}
