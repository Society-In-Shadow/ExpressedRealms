namespace ExpressedRealms.Characters.API.CharacterEndPoints.Requests;

internal record EditCharacterRequest
{
    public int Id { get; set; }

    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }

    /// <example>9</example>
    public int? FactionId { get; set; }
}
