namespace ExpressedRealms.Knowledges.UseCases.Knowledges.CreateKnowledge;

public class CreateKnowledgeModel
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int KnowledgeTypeId { get; set; }
}
