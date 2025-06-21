using ExpressedRealms.Powers.API.PowerEndpoints.Responses.PowerList;

namespace ExpressedRealms.Powers.API.PowerPathEndpoints.Responses.PowerPathList;

public class PowerPathInformationResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<PowerInformationResponse> Powers { get; set; } = new();
}
