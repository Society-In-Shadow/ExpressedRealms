namespace ExpressedRealms.Powers.API.PowerEndpoints.GetPowersForPowerPath;

public class PrerequisiteDisplay
{
    public int RequiredAmount { get; set; }
    public List<string> Powers { get; set; } = new();
}
