namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.Create;

public class CreatePlayerRequest
{
    /// <summary>
    /// Player Name
    /// </summary>
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;
}
