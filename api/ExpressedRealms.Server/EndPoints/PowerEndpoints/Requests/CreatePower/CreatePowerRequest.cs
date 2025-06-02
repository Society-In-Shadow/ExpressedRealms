namespace ExpressedRealms.Server.EndPoints.PowerEndpoints.Requests.CreatePower;

public class CreatePowerRequest
{
    public required string Name { get; set; }
    public required List<int> CategoryIds { get; set; }
    public required string Description { get; set; }
    public required string GameMechanicEffect { get; set; }
    public required string Limitation { get; set; }
    public byte PowerDuration { get; set; }
    public byte AreaOfEffect { get; set; }
    public int PowerLevel { get; set; }
    public byte PowerActivationType { get; set; }
    public required string Other { get; set; }
    public int PowerPathId { get; set; }
    public bool IsPowerUse { get; set; }
}
