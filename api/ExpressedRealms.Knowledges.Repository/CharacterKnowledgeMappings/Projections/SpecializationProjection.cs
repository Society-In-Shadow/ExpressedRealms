namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class SpecializationProjection
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Notes { get; set; }
}