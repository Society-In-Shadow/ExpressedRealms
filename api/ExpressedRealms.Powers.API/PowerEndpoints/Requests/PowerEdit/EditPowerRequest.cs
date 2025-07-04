namespace ExpressedRealms.Powers.API.PowerEndpoints.Requests.PowerEdit;

public class EditPowerRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<int>? CategoryIds { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public string? Limitation { get; set; }
    public byte PowerDurationId { get; set; }
    public byte AreaOfEffectId { get; set; }
    public int PowerLevelId { get; set; }
    public byte PowerActivationTypeId { get; set; }
    public string? Other { get; set; }
    public bool IsPowerUse { get; set; }
    public string? Cost { get; set; }
}
