namespace ExpressedRealms.Knowledges.API.EditKnowledge;

public class EditKnowledgeRequest
{
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
    public int KnowledgeTypeId { get; set; }
}
