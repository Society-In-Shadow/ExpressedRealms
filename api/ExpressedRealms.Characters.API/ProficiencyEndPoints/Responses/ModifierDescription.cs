using ExpressedRealms.Characters.Repository.Proficiencies.Enums;

namespace ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;

public class ModifierDescription
{
    public string Message { get; set; } = null!;
    public int Value { get; set; }
    public ModifierType Type { get; set; }
}