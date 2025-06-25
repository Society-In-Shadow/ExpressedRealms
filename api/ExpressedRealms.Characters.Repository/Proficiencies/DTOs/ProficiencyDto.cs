using ExpressedRealms.Characters.Repository.Enums;

namespace ExpressedRealms.Characters.Repository.DTOs;

public class ProficiencyDto
{
    public string OffensiveName { get; set; } = null!;
    public int OffensiveValue { get; set; }
    public string DefensiveName { get; set; } = null!;
    public int DefensiveValue { get; set; }
}