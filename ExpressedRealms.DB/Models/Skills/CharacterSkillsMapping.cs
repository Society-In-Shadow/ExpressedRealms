using ExpressedRealms.DB.Characters;

namespace ExpressedRealms.DB.Models.Skills;

public class CharacterSkillsMapping
{
    public int CharacterId { get; set; }
    public byte SkillTypeId { get; set; }
    public byte SkillLevelId { get; set; }

    public virtual Character Character { get; set; } = null!;
    public virtual SkillType SkillType { get; set; } = null!;
    public virtual SkillLevel SkillLevel { get; set; } = null!;
}
