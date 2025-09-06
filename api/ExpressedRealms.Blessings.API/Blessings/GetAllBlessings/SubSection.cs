namespace ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;

public class SubSection
{
    public required string Name { get; set; }
    public List<Blessing> Blessings { get; set; } = new();
}