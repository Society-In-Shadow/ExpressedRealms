namespace ExpressedRealms.Expressions.Reports.FactionReport;

public class FactionReportData
{
    public required string ExpressionName { get; set; }
    public List<FactionData> Factions { get; set; } = new();
    public bool IsExpression { get; set; }
}
