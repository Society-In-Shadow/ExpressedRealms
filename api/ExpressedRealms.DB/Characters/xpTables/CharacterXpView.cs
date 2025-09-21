namespace ExpressedRealms.DB.Characters.xpTables;

public sealed class CharacterXpView
{
    public int CharacterId { get; set; }
    public string SectionName { get; set; } = null!;
    public int SectionTypeId { get; set; }
    public int SectionCap { get; set; }
    /// <summary>
    /// Advantage / Disadvantages have 8 point soft caps, but disadvantages only count towards total xp if they are selected.
    /// </summary>
    public int TrueSectionCap { get; set; }
    /// <summary>
    /// Advantage / Disadvantages are special, advantages count toward the total, but advantages do not
    /// </summary>
    public int TrueTotalSpent { get; set; }
    public int SpentXp { get; set; }
    public int DiscretionXp { get; set; }
    public int TotalCharacterCreationXp { get; set; }
    public int LevelXp { get; set; }
}
