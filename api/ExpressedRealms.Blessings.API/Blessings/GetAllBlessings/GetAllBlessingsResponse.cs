namespace ExpressedRealms.Blessings.API.Blessings.GetAllBlessings;

public class GetAllBlessingsResponse
{
    public List<Blessing> Advantages { get; set; }
    public List<Blessing> DisAdvantages { get; set; }
    public List<Blessing> MixedBlessings { get; set; }
}