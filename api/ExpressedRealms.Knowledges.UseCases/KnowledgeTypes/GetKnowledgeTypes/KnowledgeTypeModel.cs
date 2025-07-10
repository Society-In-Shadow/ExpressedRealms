namespace ExpressedRealms.Knowledges.UseCases.KnowledgeTypes.GetKnowledgeTypes;

public class KnowledgeTypeModel
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; }
}
