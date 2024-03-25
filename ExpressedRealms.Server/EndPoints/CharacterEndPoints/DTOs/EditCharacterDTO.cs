namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;

public class EditCharacterDTO
{
    public int Id { get; set; }

    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }
}
