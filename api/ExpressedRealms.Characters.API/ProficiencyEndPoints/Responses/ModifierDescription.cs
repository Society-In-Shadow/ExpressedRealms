using ExpressedRealms.Characters.Repository.Proficiencies.Enums;

namespace ExpressedRealms.Characters.API.ProficiencyEndPoints.Responses;

public class ModifierDescription
{
    public required string Message { get; set; }
    public int Value { get; set; }
    public required ModifierType Type { get; set; }
    public required string Name { get; set; }
}
