namespace ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Create;

public class CreateKnowledgeSpecializationRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Notes { get; set; }
}
