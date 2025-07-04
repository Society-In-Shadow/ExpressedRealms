namespace ExpressedRealms.Powers.API.PowerPrerequisites.Requests.CreatePrerequisite;

public class CreatePrerequisiteRequest
{
    public required List<int> PowerIds { get; set; }
    public int RequiredAmount { get; set; }
}
