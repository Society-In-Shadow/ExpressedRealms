namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetEarlyCheckinInformationForPlayer;

public record EventInfo()
{
    public int EventId { get; set; }
    public string EventName { get; set; } = null!;
    public DateOnly StartDate { get; set; }
};