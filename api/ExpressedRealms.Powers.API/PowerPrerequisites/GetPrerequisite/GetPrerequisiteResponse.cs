namespace ExpressedRealms.Powers.API.PowerPrerequisites.GetPrerequisite;

public class GetPrerequisiteResponse
{
    public int Id { get; set; }
    public int RequiredAmount { get; set; }
    public required List<int> PowerIds { get; set; }
}
