namespace ExpressedRealms.Events.API.Repositories.EventCheckin.Dtos;

public class UserCrbEmailPreferenceDto
{
    public bool SendPickupCrbEmail { get; set; }
    public required string UserEmailAddress { get; set; }
}