namespace ExpressedRealms.Events.API.Discord;

public class DiscordEvent
{
    public required string Name { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset EndDate { get; set; }
    public required string Location { get; set; }
}