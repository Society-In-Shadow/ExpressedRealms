namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetOverallStats;

public class ExperienceBreakdownResponse
{
    public List<ExperienceSection> Experience { get; set; } = new();
}
