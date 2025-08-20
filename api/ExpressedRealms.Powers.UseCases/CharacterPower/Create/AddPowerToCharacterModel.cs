namespace ExpressedRealms.Powers.UseCases.CharacterPower.Create;

public class AddPowerToCharacterModel
{
    public int CharacterId { get; set; }
    public int PowerId { get; set; }
    public string? Notes { get; set; }
}
