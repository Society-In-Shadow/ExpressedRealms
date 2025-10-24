namespace ExpressedRealms.Events.API.UseCases.Events.Create;

public class CreateEventModel
{
    public required string Name { get; set; }
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
    public required string Location { get; set; } = null!;
    public required string WebsiteName { get; set; } = null!;
    public required string WebsiteUrl { get; set; } = null!;
    public required string AdditionalNotes { get; set; } = null!;
    public int ConExperience { get; set; }
}
