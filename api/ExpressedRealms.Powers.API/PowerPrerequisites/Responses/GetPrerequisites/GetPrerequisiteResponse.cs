namespace ExpressedRealms.Powers.API.PowerEndpoints.Responses.GetPrerequisites;

public class GetPrerequisiteResponse
{
    public int Id { get; set; }
    public int RequiredAmount { get; set; }
    public List<int> PowerIds { get; set; }
}
