namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.StatDTOs;

public class EditStatDTO
{
    public int CharacterId { get; set; }
    public StatType StatTypeId { get; set; }
    public byte LevelTypeId { get; set; }
}