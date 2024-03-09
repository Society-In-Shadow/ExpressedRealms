namespace ExpressedRealms.Server.EndPoints.PlayerEndpoints.DTOs;

public class CreatePlayerDTO
{
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
}
