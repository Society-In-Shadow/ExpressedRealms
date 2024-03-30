namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.DTOs;

public class CharacterDTO
{
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }

    /// <example>Adept</example>
    public string Expression { get; set; }
}
