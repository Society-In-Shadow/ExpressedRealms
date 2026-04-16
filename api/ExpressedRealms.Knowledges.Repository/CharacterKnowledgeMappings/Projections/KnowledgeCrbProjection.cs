namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class KnowledgeCrbProjection
{
    public required string Name { get; set; }
    public int Level { get; set; }
    public List<string> Specializations { get; set; } = new List<string>();
}
