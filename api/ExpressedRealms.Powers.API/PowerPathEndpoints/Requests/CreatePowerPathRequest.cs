namespace ExpressedRealms.Powers.API.PowerPathEndpoints.Requests;

public class CreatePowerPathRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int ExpressionId { get; set; }
}
