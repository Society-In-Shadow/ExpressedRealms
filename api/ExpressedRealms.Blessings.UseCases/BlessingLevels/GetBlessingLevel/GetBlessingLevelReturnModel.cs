namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.GetBlessingLevel;

public class GetBlessingLevelReturnModel
{
    public int Id { get; set; }
    public int BlessingId { get; set; }   
    public required string Level { get; set; }
    public required string Description { get; set; }   
    public int XpCost { get; set; }
    public int XpGain { get; set; }
}