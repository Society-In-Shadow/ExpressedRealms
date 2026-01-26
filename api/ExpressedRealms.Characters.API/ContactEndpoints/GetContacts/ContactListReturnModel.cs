namespace ExpressedRealms.Characters.API.ContactEndpoints.GetContacts;

public class Contact
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Knowledge { get; set; }
    public required string KnowledgeLevel { get; set; }
    public int UsesPerWeek { get; set; }
    public bool IsApproved { get; set; }
}