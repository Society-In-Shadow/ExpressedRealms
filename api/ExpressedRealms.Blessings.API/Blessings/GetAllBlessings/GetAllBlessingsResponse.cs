namespace ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;

public class GetAllBlessingsResponse
{
    public List<Blessing> Advantages { get; set; } = new();
    public List<Blessing> Disadvantages { get; set; } = new();
    public List<Blessing> MixedBlessings { get; set; } = new();
}
