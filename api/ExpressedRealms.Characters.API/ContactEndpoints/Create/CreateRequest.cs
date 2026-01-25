namespace ExpressedRealms.Characters.API.ContactEndpoints.Create;

public class CreateRequest
{
    public int KnowledgeId { get; set; }
    public byte ContactFrequency { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public byte KnowledgeLevel { get; set; }
}
