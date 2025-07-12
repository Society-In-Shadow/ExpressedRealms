namespace ExpressedRealms.Knowledges.API.GetKnowledge;

public class GetKnowledgeResponse
{
    public int Id { get; set; }
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
    public int TypeId { get; set; }
}
