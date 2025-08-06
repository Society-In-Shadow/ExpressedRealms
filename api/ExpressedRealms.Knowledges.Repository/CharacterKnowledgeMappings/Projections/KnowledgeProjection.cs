namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class KnowledgeProjection
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
}
