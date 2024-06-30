namespace ExpressedRealms.DB.Models.Skills;

public class SkillSubType
{
    public byte Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<SkillType> SkillTypes { get; set; } = null!;
}
