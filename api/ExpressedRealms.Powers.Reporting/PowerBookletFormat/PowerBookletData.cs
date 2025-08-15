namespace ExpressedRealms.Powers.Reporting.PowerBookletFormat;

public class PowerBookletData
{
    public required string ExpressionName { get; set; }
    public List<PowerPathData> PowerPaths { get; set; } = new();
}
