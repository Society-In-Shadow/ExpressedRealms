namespace ExpressedRealms.Characters.UseCases.Contacts.GetContact;

public class ContactReturnModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Notes { get; set; }
    public int KnowledgeId { get; set; }
    public int KnowledgeLevelId { get; set; }
    public int UsesPerWeek { get; set; }
    public bool IsApproved { get; set; }
}
