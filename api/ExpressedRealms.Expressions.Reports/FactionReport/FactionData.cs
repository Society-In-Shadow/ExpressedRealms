namespace ExpressedRealms.Expressions.Reports.FactionReport;

public class FactionData
{
    public required string Name { get; set; }
    public required string Background { get; set; }
    public List<FactionLevelData> FactionLevels { get; set; } = new();
}
