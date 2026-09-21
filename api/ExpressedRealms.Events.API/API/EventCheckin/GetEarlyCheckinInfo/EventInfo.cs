namespace ExpressedRealms.Events.API.API.EventCheckin.GetEarlyCheckinInfo;

public record EventInfo()
{
    public int EventId { get; set; }
    public string EventName { get; set; } = null!;
    public DateOnly StartDate { get; set; }
};
