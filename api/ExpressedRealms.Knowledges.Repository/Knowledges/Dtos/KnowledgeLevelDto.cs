namespace ExpressedRealms.Knowledges.Repository.Knowledges.Dtos;

public record KnowledgeLevelDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Stones { get; set; }
    public int Level { get; set; }
    public int TotalXpCost { get; set; }
    public int SpecializationCount { get; set; }
}