namespace ExpressedRealms.DB.Characters.xpTables;

public class CharacterXpMapping
{
    public int CharacterId { get; set; }
    public int SectionCap { get; set; }
    public int XpSectionTypeId { get; set; }
    public int SpentXp { get; set; }
    public int DiscretionXp { get; set; }
    public int TotalCharacterCreationXp { get; set; }
    public int LevelXp { get; set; }
    
    public Character Character { get; set; } = null!;
    public XpSectionType XpSectionType { get; set; } = null!;
}