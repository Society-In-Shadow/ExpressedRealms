namespace ExpressedRealms.Characters.Repository.Contacts.Dtos;

public class ContactListForCrbDto
{
    public required string Name { get; set; }
    public required string Knowledge { get; set; }
    public int KnowledgeLevel { get; set; }
    public int UsesPerWeek { get; set; }
}
