namespace ExpressedRealms.Events.API.API.Events.Create;

public class CreateEventRequest
{
    public required string Name { get; set; }
    public required DateTimeOffset StartDate { get; set; }
    public required DateTimeOffset EndDate { get; set; }
    public required string Location { get; set; } = null!;
    public required string WebsiteName { get; set; } = null!;
    public required string WebsiteUrl { get; set; } = null!;
    public required string AdditionalNotes { get; set; } = null!;
    public int ConExperience { get; set; }
}
