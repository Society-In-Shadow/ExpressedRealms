namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class CharacterKnowledgeProjection
{
    public int MappingId { get; set; }
    public required string LevelName { get; set; }
    public int StoneModifier { get; set; }
    public KnowledgeProjection Knowledge { get; set; }
    public List<SpecializationProjection> Specializations { get; set; }
    public int Level { get; set; }
}
