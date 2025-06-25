namespace ExpressedRealms.Characters.Repository.Skills.DTOs;

public class EditCharacterSkillMappingDto
{
    public int CharacterId { get; set; }
    public byte SkillTypeId { get; set; }
    public byte SkillLevelId { get; set; }
}
