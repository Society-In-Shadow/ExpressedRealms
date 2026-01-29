namespace ExpressedRealms.Characters.UseCases.Contacts.GetContactsForCharacterSheet;

public class ContactListReturnModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Knowledge { get; set; }
    public required string KnowledgeLevel { get; set; }
    public required string KnowledgeDescription { get; set; }
    public string? Notes { get; set; }
    public int UsesPerWeek { get; set; }
    public bool IsApproved { get; set; }
}
