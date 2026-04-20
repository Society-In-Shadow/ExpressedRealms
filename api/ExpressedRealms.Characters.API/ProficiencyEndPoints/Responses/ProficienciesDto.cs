namespace ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;

public class ProficienciesDto
{
    public string Name { get; set; } = null!;
    public List<ModifierDescription> AppliedModifiers { get; set; } = new();
    public int Value { get; set; }
    public int Id { get; set; }
}
