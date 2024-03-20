namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;

public class EditCharacterDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
}
