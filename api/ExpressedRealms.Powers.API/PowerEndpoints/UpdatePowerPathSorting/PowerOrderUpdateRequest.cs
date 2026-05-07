using ExpressedRealms.Powers.API.PowerPathEndpoints.UpdatePowerPathSorting;

namespace ExpressedRealms.Powers.API.PowerEndpoints.UpdatePowerPathSorting;

public class PowerOrderUpdateRequest
{
    public int PowerPathId { get; set; }
    public List<IdOrderDto> Items { get; set; } = new();
}
