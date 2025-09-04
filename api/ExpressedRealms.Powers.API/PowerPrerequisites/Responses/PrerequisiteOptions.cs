using ExpressedRealms.Powers.API.PowerEndpoints.Responses.Options;

namespace ExpressedRealms.Powers.API.PowerPrerequisites.Responses;

public class PrerequisiteOptions
{
    public List<DetailedEditInformation> RequiredAmount { get; set; } = new();
    public List<DetailedEditInformation> PrerequisitePowers { get; set; } = new();
}
