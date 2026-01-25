namespace ExpressedRealms.Characters.UseCases.Contacts.Create;

public class CreateContactModel
{
    public int CharacterId { get; set; }
    public int KnowledgeId { get; set; }
    public byte ContactFrequency { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public byte KnowledgeLevel { get; set; }
}
