namespace ExpressedRealms.DB.Models.Skills;

public class SkillLevelBenefit
{
    public byte SkillTypeId { get; set; }
    public byte SkillLevelId { get; set; }
    public byte Modifier { get; set; }
    public byte ModifierTypeId { get; set; }

    public virtual SkillType SkillType { get; set; }
    public virtual SkillLevel SkillLevel { get; set; }
    public virtual ModifierType ModifierType { get; set; }
}
