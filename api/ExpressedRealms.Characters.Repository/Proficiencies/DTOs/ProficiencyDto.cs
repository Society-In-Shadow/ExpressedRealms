using ExpressedRealms.Characters.Repository.Proficiencies.Enums;

namespace ExpressedRealms.Characters.Repository.Proficiencies.DTOs;

public class ProficiencyDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<ModifierType> Modifiers { get; set; } = new();
    public List<ModifierDescription> AppliedModifiers { get; set; } = new();
    public byte SortOrder { get; set; }
    public int Value => AppliedModifiers.Sum(x => x.Value);
    public required string Type { get; set; }
    public int Id { get; set; }
}
