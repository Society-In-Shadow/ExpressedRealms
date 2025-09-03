namespace ExpressedRealms.Blessings.UseCases.BlessingLevels.EditBlessingLevel;

public class EditBlessingLevelModel
{
    public int BlessingId { get; set; }
    public int LevelId { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }
    public required string Level { get; set; }
    public required string Description { get; set; }
}