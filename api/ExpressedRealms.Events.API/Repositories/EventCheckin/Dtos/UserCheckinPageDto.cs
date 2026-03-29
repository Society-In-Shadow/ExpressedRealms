namespace ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;

public class UserCheckinPageDto
{
    public bool SendPickupCrbEmail { get; set; }
    public int? CheckinId { get; set; }
    public required string LookupId { get; set; }
}