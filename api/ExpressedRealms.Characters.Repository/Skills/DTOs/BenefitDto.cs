namespace ExpressedRealms.Characters.Repository.Skills.DTOs;

public class BenefitDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte Modifier { get; set; }
    public byte LevelId { get; set; }
}
