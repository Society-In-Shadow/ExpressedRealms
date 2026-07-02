namespace ExpressedRealms.Powers.UseCases.Powers.CreatePower;

public class CreatePowerModel
{
    public CreatePowerTarget Target { get; set; }
    public int TargetId { get; set; }
    public required string Name { get; set; }
    public List<int> Category { get; set; } = new();
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public byte PowerDuration { get; set; }
    public byte AreaOfEffect { get; set; }
    public int PowerLevel { get; set; }
    public byte PowerActivationType { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
}
