namespace ExpressedRealms.DB.Models.Skills;

public class SkillLevelDescriptionMapping
{
    public byte SkillLevelId { get; set; }
    public byte SkillTypeId { get; set; }
    public string Description { get; set; }

    public virtual SkillLevel SkillLevel { get; set; } = null!;
    public virtual SkillType SkillType { get; set; } = null!;
}
