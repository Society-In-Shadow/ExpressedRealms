namespace ExpressedRealms.Blessings.API.BlessingLevels.GetAllBlessings;

public class BlessingLevelResponse
{
    public int Id { get; set; }
    public required string Level { get; set; }
    public required string Description { get; set; }
    public int XpCost { get; set; }
    public int XpGain { get; set; }
    public int? ModifierGroupId { get; set; }
}
