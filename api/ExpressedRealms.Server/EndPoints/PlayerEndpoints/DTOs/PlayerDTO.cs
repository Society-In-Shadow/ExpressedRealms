namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;

public class PlayerDTO
{
    /// <summary>
    /// Player Name
    /// </summary>
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <summary>
    /// 10 Digit Phone Number
    /// </summary>
    /// <example>(555) 555-5555</example>
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// Player Location
    /// </summary>
    /// <example>Chicago</example>
    public string City { get; set; } = null!;

    /// <summary>
    /// State Abbreviation
    /// </summary>
    /// <example>IL</example>
    public string State { get; set; } = null!;
}
