namespace ExpressedRealms.Powers.Reporting.powerCards;

public class PowerCardData
{
    public required string ExpressionName { get; set; }
    public required string PathName { get; set; }
    public int Id { get; set; }
    public required string Name { get; set; }
    public required List<string>? Category { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public required string PowerDuration { get; set; }
    public required string AreaOfEffect { get; set; }
    public required string PowerLevel { get; set; }
    public required string PowerActivationType { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
    public int PrerequisitesNeeded { get; set; }
    public string? UserNotes { get; set; }
    public PrerequisiteData? Prerequisites { get; set; }
}
