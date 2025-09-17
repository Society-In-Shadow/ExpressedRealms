namespace ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;

public class GetAllBlessingsResponse
{
    public List<SubSection> Advantages { get; set; } = new();
    public List<SubSection> Disadvantages { get; set; } = new();
}
