namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class CharacterKnowledgeProjection
{
    public int MappingId { get; set; }
    public required string LevelName { get; set; }
    public int StoneModifier { get; set; }
    public required KnowledgeProjection Knowledge { get; set; }
    public List<SpecializationProjection> Specializations { get; set; } = new();
    public int Level { get; set; }
    public string? Notes { get; set; }
    public int LevelId { get; set; }
    public int SpecializationCount { get; set; }
}
