namespace ExpressedRealms.Blessings.UseCases.CharacterBlessingMappings.Edit;

public class UpdateBlessingForCharacterModel
{
    public int BlessingLevelId { get; set; }
    public int CharacterId { get; set; }
    public int BlessingId { get; set; }
    public string? Notes { get; set; }
    public int MappingId { get; set; }
}
