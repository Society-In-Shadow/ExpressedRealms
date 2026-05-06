using ExpressedRealms.Powers.API.PowerEndpoints.GetPowersForPowerPath;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.GetPowerPathsForExpression;

public class PowerPathInformationResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<PowerInformationResponse> Powers { get; set; } = new();
}
