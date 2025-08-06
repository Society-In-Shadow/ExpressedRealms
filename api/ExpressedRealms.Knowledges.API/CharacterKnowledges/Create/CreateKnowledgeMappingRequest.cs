namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.Create;

public class CreateKnowledgeMappingRequest
{
    public int KnowledgeId { get; set; }
    public int KnowledgeLevelId { get; set; }
    public string? Notes { get; set; }
}
