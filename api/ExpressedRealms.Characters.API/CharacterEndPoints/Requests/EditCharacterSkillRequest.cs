namespace ExpressedRealms.Characters.API.CharacterEndPoints.Requests;

public class EditCharacterSkillRequest
{
    public int CharacterId { get; set; }
    public byte SkillTypeId { get; set; }
    public byte SkillLevelId { get; set; }
}
