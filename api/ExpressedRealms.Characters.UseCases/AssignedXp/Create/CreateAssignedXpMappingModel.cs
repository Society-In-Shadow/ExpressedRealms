namespace ExpressedRealms.Characters.UseCases.AssignedXp.Create;

public class CreateAssignedXpMappingModel
{
    public int EventId { get; set; }
    public int AssignedXpTypeId { get; set; }
    public int CharacterId { get; set; }
    public string? Reason { get; set; }
}
