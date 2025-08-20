namespace ExpressedRealms.Powers.API.CharacterPowerEndpoints.Create;

public class CreatePowerMappingRequest
{
    public int PowerId { get; set; }
    public int PowerLevelId { get; set; }
    public string? Notes { get; set; }
}
