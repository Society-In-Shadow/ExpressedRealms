namespace ExpressedRealms.Blessings.UseCases.Blessings.GetBlessings;

public class BlessingLevelReturnModel
{
    public int Id { get; set; }
    public required string Level { get; set; }
    public required string Description { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }
}
