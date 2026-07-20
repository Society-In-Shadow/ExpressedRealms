namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class SimpleCharacterKnowledgeProjection
{
    public int Id { get; set; }
    public int Level { get; set; }
    public List<string> Specializations { get; set; } = new();
}
