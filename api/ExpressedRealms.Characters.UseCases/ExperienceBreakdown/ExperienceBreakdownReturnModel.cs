namespace ExpressedRealms.Characters.UseCases.ExperienceBreakdown;

public class ExperienceBreakdownReturnModel
{
    public int KnowledgeXp { get; set; }
    public int SkillsXp { get; set; }
    public int PowersXp { get; set; }
    public int StatsXp { get; set; }
    public int SetupKnowledgeXp { get; set; }
    public int SetupPowersXp { get; set; }
    public int SetupStatsXp { get; set; }
    public int SetupSkillsXp { get; set; }
    public int Total { get; set; }
    public int SetupTotal { get; set; }
}