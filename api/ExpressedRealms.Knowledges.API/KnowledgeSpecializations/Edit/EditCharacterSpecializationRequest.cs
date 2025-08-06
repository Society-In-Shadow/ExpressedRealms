namespace ExpressedRealms.Knowledges.API.KnowledgeSpecializations.Edit;

public class EditCharacterSpecializationRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? Notes { get; set; }

}
