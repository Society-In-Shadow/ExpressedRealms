namespace ExpressedRealms.Events.API.UseCases.EventCheckin.UpdateAgeInformation;

public class UpdateAgeInformationModel
{
    public required string LookupId { get; set; }
    public int AgeGroupId { get; set; }
    public bool HasSignedConsentForm { get; set; }
}
