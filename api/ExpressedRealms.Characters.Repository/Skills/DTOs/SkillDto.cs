namespace ExpressedRealms.Characters.Repository.Skills.DTOs;

public class SkillDto
{
    public byte SkillTypeId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte LevelId { get; set; }
    public string LevelName { get; set; } = null!;
    public string LevelDescription { get; set; } = null!;
    public byte SkillSubTypeId { get; set; }
    public int XP { get; set; }
    public byte LevelNumber { get; set; }
    public int TotalXp { get; set; }
}
