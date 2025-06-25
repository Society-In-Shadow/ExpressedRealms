namespace ExpressedRealms.Characters.Repository.Skills.DTOs;

public class SkillLevelOptionsDto
{
    public int SkillTypeId { get; set; }
    public string Name { get; set; } = null!;
    public object Description { get; set; } = null!;
    public int LevelId { get; set; }

    public List<BenefitDto> Benefits { get; set; } = new();
    public int ExperienceCost { get; set; }
}
