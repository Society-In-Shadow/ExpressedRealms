namespace ExpressedRealms.Events.API.API.Events.Get;

public class EventModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required string Location { get; set; } = null!;
    public required string WebsiteName { get; set; } = null!;
    public required string WebsiteUrl { get; set; } = null!;
    public required string AdditionalNotes { get; set; } = null!;
    public required string TimeZoneId { get; set; }
    public int ConExperience { get; set; }
    public bool IsPublished { get; set; }
}
