namespace ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;

public class ProficienciesDto
{
    public string OffensiveName { get; set; } = null!;
    public int OffensiveValue { get; set; }
    public string DefensiveName { get; set; } = null!;
    public int DefensiveValue { get; set; }
}