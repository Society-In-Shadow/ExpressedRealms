namespace ExpressedRealms.Events.API.Repositories.Events.Dtos;

public class EventXpDto
{
    public string Name { get; set; } = string.Empty;
    public int ConExperience { get; set; }
    public DateOnly StartDate { get; set; }
}