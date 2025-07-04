namespace ExpressedRealms.Powers.Repository.PowerPrerequisites.GetPrerequisiteUseCase;

public class GetPrerequisiteData
{
    public int Id { get; set; }
    public int RequiredAmount { get; set; }
    public List<int> PowerIds { get; set; } = new();
}
