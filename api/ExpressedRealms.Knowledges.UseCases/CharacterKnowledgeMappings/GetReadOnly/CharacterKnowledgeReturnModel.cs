namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class CharacterKnowledgeReturnModel
{
    public int MappingId { get; set; }
    public required string LevelName { get; set; }
    public int StoneModifier { get; set; }
    public KnowledgeReturnModel Knowledge { get; set; }
    public List<SpecializationReturnModel> Specializations { get; set; }
    public int Level { get; set; }
}
