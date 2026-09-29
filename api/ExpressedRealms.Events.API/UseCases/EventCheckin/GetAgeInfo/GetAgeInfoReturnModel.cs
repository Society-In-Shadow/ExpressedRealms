namespace ExpressedRealms.Events.API.UseCases.EventCheckin.GetAgeInfo;

public class GetAgeInfoReturnModel
{
    public int? AgeGroupId { get; set; }
    public bool HasBeenVerified { get; set; }
    public required string PlayerName { get; set; }
}
