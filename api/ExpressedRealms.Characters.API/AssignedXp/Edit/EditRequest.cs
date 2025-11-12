namespace ExpressedRealms.Characters.API.AssignedXp.Edit;

public class EditRequest
{
    public int EventId { get; set; }
    public int AssignedXpTypeId { get; set; }
    public string? Reason { get; set; }
    public int Amount { get; set; }
}
