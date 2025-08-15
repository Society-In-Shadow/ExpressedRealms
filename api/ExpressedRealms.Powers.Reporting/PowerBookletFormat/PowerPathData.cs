namespace ExpressedRealms.Powers.Reporting.PowerBookletFormat;

public class PowerPathData
{
    public required string ExpressionName { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public List<PowerData> Powers { get; set; } = new();
}