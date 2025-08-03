using ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;

public class CharacterKnowledgeReturnModel
{
    public int MappingId { get; set; }
    public required string LevelName { get; set; }
    public int StoneModifier { get; set; }
    public required KnowledgeReturnModel Knowledge { get; set; }
    public List<SpecializationReturnModel> Specializations { get; set; } = new();
    public int Level { get; set; }
}
