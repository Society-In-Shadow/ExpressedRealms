namespace ExpressedRealms.Expressions.Repository.Factions.Dtos;

public class FactionLevelListDto
{
    public int Id { get; set; }
    public required string RankName { get; set; }
    public int? KnowledgeId { get; set; }
    public string? Knowledge { get; set; }
    public string? KnowledgeLevel { get; set; }
    public string? Specialization { get; set; }
    public int? KnowledgeLevelId { get; set; }
}
