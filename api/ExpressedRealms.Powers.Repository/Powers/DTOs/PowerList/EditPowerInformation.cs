namespace ExpressedRealms.Powers.Repository.Powers.DTOs.PowerList;

public class EditPowerInformation
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required List<int> CategoryIds { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public int PowerDurationId { get; set; }
    public int AreaOfEffectId { get; set; }
    public int PowerLevelId { get; set; }
    public int PowerActivationTypeId { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
}
