namespace ExpressedRealms.Server.EndPoints.DTOs;

public class CreateCharacterDTO
{
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }

    /// <example>1 - Adept</example>
    public int ExpressionId { get; set; }
}
