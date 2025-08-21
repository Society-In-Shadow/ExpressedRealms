namespace ExpressedRealms.Powers.UseCases.CharacterPower.Edit;

public class UpdatePowerForCharacterModel
{
    public int CharacterId { get; set; }
    public int PowerId { get; set; }
    public string? Notes { get; set; }
}
