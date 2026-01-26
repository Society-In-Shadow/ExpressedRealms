namespace ExpressedRealms.Characters.API.ContactEndpoints.Edit;

public class EditRequest
{
    public byte ContactFrequency { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public byte KnowledgeLevel { get; set; }
}
