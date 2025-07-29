namespace ExpressedRealms.Blessings.UseCases.Blessings.GetBlessings;

public class BlessingReturnModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public string? SubCategory { get; set; }
    public List<BlessingLevelReturnModel> Levels { get; set; } = new ();
}
