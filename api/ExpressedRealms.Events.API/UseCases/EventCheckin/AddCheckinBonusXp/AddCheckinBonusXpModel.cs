namespace ExpressedRealms.Events.API.UseCases.EventCheckin.AddCheckinBonusXp;

public class AddCheckinBonusXpModel
{
    public required string LookupId { get; set; }
    public int AssignedXpTypeId { get; set; }
    public int Amount { get; set; }
}
