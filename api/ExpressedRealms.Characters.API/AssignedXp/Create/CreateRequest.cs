namespace ExpressedRealms.Characters.API.AssignedXp.Create;

public class CreateRequest
{
    public int EventId { get; set; }
    public int AssignedXpTypeId { get; set; }
    public string? Reason { get; set; }
}
