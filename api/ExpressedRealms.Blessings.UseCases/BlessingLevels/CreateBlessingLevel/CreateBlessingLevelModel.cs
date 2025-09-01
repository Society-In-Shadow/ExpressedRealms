namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.CreateBlessingLevel;

public class CreateBlessingLevelModel
{
    public int BlessingId { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }
    public required string Level { get; set; }
    public required string Description { get; set; }
}