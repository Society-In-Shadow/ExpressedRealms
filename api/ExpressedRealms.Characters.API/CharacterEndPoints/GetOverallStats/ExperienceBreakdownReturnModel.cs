namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetOverallStats;

public class ExperienceBreakdownResponse
{
    public List<ExperienceSection> Experience { get; set; } = new();
    public int AvailableDiscretionary { get; set; }
    public int TotalSpentLevelXp { get; set; }
    public int TotalAvailableXp { get; set; }
}
