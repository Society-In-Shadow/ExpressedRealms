namespace ExpressedRealms.DB.Characters.xpTables;

public sealed class CharacterXpView
{
    public int CharacterId { get; set; }
    public string SectionName { get; set; } = null!;
    public int SectionTypeId { get; set; }
    public int SectionCap { get; set; }
    public int SpentXp { get; set; }
    public int DiscretionXp { get; set; }
    public int TotalCharacterCreationXp { get; set; }
    public int LevelXp { get; set; }
}
