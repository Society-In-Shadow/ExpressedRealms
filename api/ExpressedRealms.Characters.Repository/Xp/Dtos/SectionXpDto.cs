namespace ExpressedRealms.Characters.Repository.Xp.Dtos;

public class SectionXpDto
{
    /// <summary>
    /// This is the amount of XP available to spend for a given section.
    /// This will take into consideration XP spent in other sections to make sure that you are not going
    /// over the total amount of assigned xp / creation cap
    /// </summary>
    public int AvailableXp { get; set; }

    /// <summary>
    /// This is the total amount of xp spent for the given section.
    /// Eg, 14 xp across 2 knowledges
    /// </summary>
    public int SpentXp { get; set; }
}
