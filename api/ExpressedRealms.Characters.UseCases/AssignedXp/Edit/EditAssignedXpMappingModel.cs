namespace ExpressedRealms.Characters.UseCases.AssignedXp.Edit;

public class EditAssignedXpMappingModel
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int AssignedXpTypeId { get; set; }
    public int Amount { get; set; }
    public string? Reason { get; set; }
    public int CharacterId { get; set; }
}
