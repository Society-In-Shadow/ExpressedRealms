namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

public class ExperienceBreakdownReturnModel
{
    public List<ExperienceTotalMax> ExperienceSections { get; set; } = new();
    public int AvailableDiscretionary { get; set; }
    public int TotalSpentLevelXp { get; set; }
    public int TotalAvailableXp { get; set; }
}
