namespace ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;

public class BasicFactionLevelProjection
{
    public int Id { get; set; }
    public int? KnowledgeLevel { get; set; }
    public string? Specialization { get; set; }
    public int FactionRankId { get; set; }
    public int? KnowledgeId { get; set; }
}
