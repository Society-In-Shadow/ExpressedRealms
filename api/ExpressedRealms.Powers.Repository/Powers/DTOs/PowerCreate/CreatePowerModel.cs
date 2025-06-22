namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerCreate;

public class CreatePowerModel
{
    public required string Name { get; set; }
    public List<int>? Category { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public byte PowerDuration { get; set; }
    public byte AreaOfEffect { get; set; }
    public int PowerLevel { get; set; }
    public byte PowerActivationType { get; set; }
    public string? Other { get; set; }
    public int PowerPathId { get; set; }
    public bool IsPowerUse { get; set; }
}
