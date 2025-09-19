namespace ExpressedRealms.DB.Characters.xpTables;

public class XpSectionType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int SectionCap { get; set; }

    public virtual List<CharacterXpMapping> CharacterXpMappings { get; set; } = null!;
}
