namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Create;

public class AddBlessingToCharacterModel
{
    public int BlessingLevelId { get; set; }
    public int CharacterId { get; set; }
    public int BlessingId { get; set; }
    public string? Notes { get; set; }
}
