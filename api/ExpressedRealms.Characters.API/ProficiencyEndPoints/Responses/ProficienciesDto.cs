using ExpressedRealms.Characters.Repository.Proficiencies.Enums;

namespace ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;

public class ProficienciesDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ModifierType> Modifiers { get; set; } = new();
    public List<ModifierDescription> AppliedModifiers { get; set; } = new();
    public byte CorrespondingId { get; set; }
    public int Value { get; set; }
    public int Id { get; set; }
    public required string Type { get; set; }
}
