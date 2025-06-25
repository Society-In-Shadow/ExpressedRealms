using ExpressedRealms.Characters.Repository.Proficiencies.Enums;

namespace ExpressedRealms.Characters.Repository.Proficiencies.DTOs;

public class ModifierDescription
{
    public string Message { get; set; } = null!;
    public int Value { get; set; }
    public ModifierType Type { get; set; }
}