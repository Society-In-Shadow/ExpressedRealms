namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public record CharacterKnowledgeMappingProjection()
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    public int SelectedLevelId { get; set; }
    public bool IsUnknownType { get; set; }
    public int CharacterId { get; set; }
    public int KnowledgeId { get; set; }
    public string? Notes { get; set; }
};
