namespace ExpressedRealms.Powers.API.PowerPrerequisites.Requests.CreatePrerequisite;

public class CreatePrerequisiteRequest
{
    public List<int> PowerIds { get; set; }
    public int RequiredAmount { get; set; }
}
