namespace ExpressedRealms.Events.API.API.EventScheduleItem.Edit;

public class EditEventScheduleItemRequest
{
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required DateTime StartTime { get; set; }
    public required DateTime EndTime { get; set; }
}
