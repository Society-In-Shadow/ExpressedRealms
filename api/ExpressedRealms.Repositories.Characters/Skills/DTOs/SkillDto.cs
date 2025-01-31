namespace ExpressedRealms.Repositories.Characters.Skills.DTOs;

public class SkillDto
{
    public byte SkillTypeId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte LevelId { get; set; }
    public string LevelName { get; set; }
}