namespace ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;

public class Level
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }

}
