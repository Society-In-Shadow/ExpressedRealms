namespace ExpressedRealms.Server.EndPoints.CharacterEndPoints.Responses;

public class CharacterEditResponse
{
    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }

    /// <example>Adept</example>
    public string Expression { get; set; }

    /// <example>8</example>
    public int FactionId { get; set; }
}
