using ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.GetAll;

public class CharacterKnowledgeResponse
{
    public int MappingId { get; set; }
    public required string LevelName { get; set; }
    public int StoneModifier { get; set; }
    public required KnowledgeModel Knowledge { get; set; }
    public List<SpecializationModel> Specializations { get; set; } = new();
    public int Level { get; set; }
}
