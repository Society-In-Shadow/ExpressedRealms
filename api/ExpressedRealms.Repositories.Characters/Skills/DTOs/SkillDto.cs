namespace ExpressedRealms.Repositories.Characters.Skills.DTOs;

public class SkillDto
{
    public byte SkillTypeId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte LevelId { get; set; }
    public string LevelName { get; set; } = null!;
    public string LevelDescription { get; set; } = null!;
    public byte SkillSubTypeId { get; set; }
    public List<BenefitDto> Benefits { get; set; } = new();
}
