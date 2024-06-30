namespace ExpressedRealms.DB.Models.Skills;

public class SkillType
{
    public byte Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte SkillSubTypeId { get; set; }

    public virtual SkillSubType SkillSubType { get; set; } = null!;
    public virtual List<SkillLevelBenefit> SkillLevelBenefits { get; set; } = null!;
    public virtual List<CharacterSkillsMapping> CharacterSkillsMappings { get; set; } = null!;
}
