namespace ExpressedRealms.DB.Models.Skills;

public class SkillLevel
{
    public byte Id { get; set; }
    public byte Level { get; set; }
    public string Name { get; set; } = null!;
    public int XP { get; set; }

    public virtual List<SkillLevelBenefit> SkillLevelBenefits { get; set; } = null!;
    public virtual List<CharacterSkillsMapping> CharacterSkillsMappings { get; set; } = null!;
}
