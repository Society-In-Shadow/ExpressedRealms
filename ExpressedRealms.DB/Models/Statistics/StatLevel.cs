using ExpressedRealms.DB.Characters;

namespace ExpressedRealms.DB.Models.Statistics;

public class StatLevel
{
    public byte Id { get; set; }
    public int Bonus { get; set; }
    public int XPCost { get; set; }
    public int TotalXPCost { get; set; }

    public virtual List<StatDescriptionMapping> StatDescriptionMappings { get; set; } = null!;
    public virtual List<Character> CharacterAgility { get; set; } = null!;
    public virtual List<Character> CharacterConstitution { get; set; } = null!;
    public virtual List<Character> CharacterDexterity { get; set; } = null!;
    public virtual List<Character> CharacterStrength { get; set; } = null!;
    public virtual List<Character> CharacterIntelligence { get; set; } = null!;
    public virtual List<Character> CharacterWillpower { get; set; } = null!;
}
