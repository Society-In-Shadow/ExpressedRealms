namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetEditKnowledge;

public class KnowledgeTypeModel
{
    public int Id { get; set; }
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; }
}
