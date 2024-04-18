namespace ExpressedRealms.DB.Models.Statistics;

public class StatDescriptionMapping
{
    public byte StatTypeId { get; set; }
    public byte StatLevelId { get; set; }
    public string ReasonableExpectation { get; set; }

    public StatLevel StatLevel { get; set; }
    public StatType StatType { get; set; }
}
