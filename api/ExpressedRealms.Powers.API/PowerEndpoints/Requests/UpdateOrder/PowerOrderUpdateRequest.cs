using ExpressedRealms.Powers.API.PowerPathEndpoints.Requests;

namespace ExpressedRealms.Powers.API.PowerEndpoints.Requests.UpdateOrder;

public class PowerOrderUpdateRequest
{
    public int PowerPathId { get; set; }
    public List<IdOrderDto> Items { get; set; } = new();
}
