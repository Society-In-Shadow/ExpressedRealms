namespace ExpressedRealms.Knowledges.API.CreateKnowledge;

public class CreateKnowledgeRequest
{
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
    public int KnowledgeTypeId { get; set; }
}
