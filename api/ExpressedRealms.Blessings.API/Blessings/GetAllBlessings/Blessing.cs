namespace ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;

public class Blessing
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? SubCategory { get; set; }
    public List<Level> Levels { get; set; } = new();
}
