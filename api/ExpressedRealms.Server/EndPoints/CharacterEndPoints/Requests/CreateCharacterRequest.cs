namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.Requests;

public record CreateCharacterRequest
{
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }

    /// <example>3 - Adept</example>
    public int ExpressionId { get; set; }

    /// <example>9 - The Shield Wardens</example>
    public int? FactionId { get; set; }
}
