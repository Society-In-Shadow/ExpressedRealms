namespace ExpressedRealms.Server.EndPoints.DTOs;

public class CreateCharacterDTO
{
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
}
