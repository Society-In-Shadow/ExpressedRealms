namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetReadOnly;

public class KnowledgeReturnModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public int Id { get; set; }
}
