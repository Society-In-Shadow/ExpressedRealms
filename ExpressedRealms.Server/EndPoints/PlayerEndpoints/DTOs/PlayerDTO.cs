namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;

public class PlayerDTO
{
    /// <summary>
    /// Name of player
    /// </summary>
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;
    /// <summary>
    /// 9 digit phone number
    /// </summary>
    /// <example>(555) 555-5555</example>
    public string PhoneNumber { get; set; } = null!;
    /// <summary>
    /// City where the player is
    /// </summary>
    /// <example>Chicago</example>
    public string City { get; set; } = null!;
    /// <summary>
    /// 2 character state name
    /// </summary>
    /// <example>IL</example>
    public string State { get; set; } = null!;
}
