namespace ExpressedRealms.Characters.UseCases.Contacts.Edit;

public class EditContactModel
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public byte ContactFrequency { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public byte KnowledgeLevel { get; set; }
}
