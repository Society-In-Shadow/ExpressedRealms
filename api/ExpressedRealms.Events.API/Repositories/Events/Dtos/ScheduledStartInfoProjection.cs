namespace ExpressedRealms.Events.API.Repositories.Events.Dtos;

public class ScheduledStartInfoProjection
{
    public int EventId { get; set; }
    public string EventName { get; set; } = null!;
    public DateOnly StartDate { get; set; }
}
