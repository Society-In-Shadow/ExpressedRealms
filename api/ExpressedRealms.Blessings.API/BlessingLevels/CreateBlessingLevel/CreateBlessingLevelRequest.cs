namespace ExpressedRealms.Blessings.API.BlessingLevels.CreateBlessingLevel;

public class CreateBlessingLevelRequest
{
    public required string Description { get; set; }
    public required string Level { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }
}
