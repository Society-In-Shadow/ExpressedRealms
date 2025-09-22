namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetOverallStats;

public class ExperienceSection
{
    public required string Name { get; set; }
    public int Total { get; set; }
    public int CharacterCreateMax { get; set; }
    public int SectionTypeId { get; set; }
    public int LevelXp { get; set; }
}
