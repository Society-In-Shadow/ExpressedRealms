namespace ExpressedRealms.Repositories.Characters.DTOs;

public sealed record CharacterListDto
{
    public string Id { get; set; }

    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }

    /// <example>Adept</example>
    public string Expression { get; set; }
}