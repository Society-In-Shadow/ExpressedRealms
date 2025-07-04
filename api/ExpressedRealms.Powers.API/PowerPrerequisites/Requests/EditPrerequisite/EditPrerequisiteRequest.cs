namespace ExpressedRealms.Powers.API.PowerPrerequisites.Requests.EditPrerequisite;

public class EditPrerequisiteRequest
{
    public int Id { get; set; }
    public List<int> PowerIds { get; set; }
    public int RequiredAmount { get; set; }
}
