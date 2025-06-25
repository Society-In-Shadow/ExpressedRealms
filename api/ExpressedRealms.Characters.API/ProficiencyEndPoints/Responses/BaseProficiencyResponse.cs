namespace ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;

internal class BaseProficiencyResponse
{
    public List<ProficienciesDto> Proficiencies { get; set; } = new();
}
