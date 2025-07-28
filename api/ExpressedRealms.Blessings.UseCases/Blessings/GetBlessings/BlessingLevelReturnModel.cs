namespace ExpressedRealms.Blessings.UseCases.Blessings.GetBlessings;

public class BlessingLevelReturnModel
{
    public string Level { get; set; }
    public string Description { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }
}