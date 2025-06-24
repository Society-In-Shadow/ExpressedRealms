namespace ExpressedRealms.Powers.API.PowerPathEndpoints.Requests;

public class PowerPathOrderUpdateRequest
{
    public int ExpressionId { get; set; }
    public List<IdOrderDto> Items { get; set; } = new();
}
