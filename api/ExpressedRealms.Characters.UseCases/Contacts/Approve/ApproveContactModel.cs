namespace ExpressedRealms.Characters.UseCases.Contacts.Approve;

public class ApproveContactModel
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public bool Approved { get; set; }
}
