namespace ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;

internal class BaseProficiencyResponse
{
    public List<ProficienciesDto> Offensive { get; set; } = new();
    public List<ProficienciesDto> Defensive { get; set; } = new();
    public List<ProficienciesDto> Secondary { get; set; } = new();
}
