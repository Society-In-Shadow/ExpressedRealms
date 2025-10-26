namespace ExpressedRealms.Events.API.UseCases.EventScheduleItems.Create;

public class CreateEventScheduleItemModel
{
    public int EventId { get; set; }
    public required string Description { get; set; }
    public required DateOnly Date { get; set; }
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
}
