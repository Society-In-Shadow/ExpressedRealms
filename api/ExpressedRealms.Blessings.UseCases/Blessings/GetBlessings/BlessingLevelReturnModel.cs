namespace ExpressedRealms.Blessings.UseCases.Blessings.GetBlessings;

public class BlessingLevelReturnModel
{
    public required string Level { get; set; }
    public required string Description { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }
}
