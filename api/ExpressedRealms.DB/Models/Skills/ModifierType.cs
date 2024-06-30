namespace ExpressedRealms.DB.Models.Skills;

public class ModifierType
{
    public byte Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<SkillLevelBenefit> SkillLevelBenefits { get; set; } = null!;
}
