namespace ExpressedRealms.Characters.Repository.DTOs;

public sealed record CharacterListDto
{
    public string Id { get; set; } = null!;

    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>Adept</example>
    public string Expression { get; set; } = null!;

    public bool IsPrimaryCharacter { get; set; }
    public bool IsInCharacterCreate { get; set; }
    public bool IsRetired { get; set; }
}
