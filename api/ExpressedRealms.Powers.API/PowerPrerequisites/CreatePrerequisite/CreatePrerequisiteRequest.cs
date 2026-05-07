namespace ExpressedRealms.Powers.API.PowerPrerequisites.CreatePrerequisite;

public class CreatePrerequisiteRequest
{
    public required List<int> PowerIds { get; set; }
    public int RequiredAmount { get; set; }
}
