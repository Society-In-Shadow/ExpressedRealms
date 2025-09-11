namespace ExpressedRealms.Blessings.API.CharacterBlessings.Create;

public class CreateBlessingMappingRequest
{
    public int BlessingId { get; set; }
    public int BlessingLevelId { get; set; }
    public string? Notes { get; set; }
}
