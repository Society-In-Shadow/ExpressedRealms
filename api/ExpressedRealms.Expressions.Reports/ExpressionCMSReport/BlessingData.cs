namespace ExpressedRealms.Expressions.Reports.ExpressionCMSReport;

public class BlessingData
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public string? SubType { get; set; }
    public required string Description { get; set; }
    public List<BlessingLevelData> Levels { get; set; } = new();
}