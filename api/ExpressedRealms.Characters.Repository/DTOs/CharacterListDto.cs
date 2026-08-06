namespace ExpressedRealms.Characters.Repository.DTOs;

public sealed record CharacterListDto
{
    public int Id { get; set; }

    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>Adept</example>
    public string ExpressionName { get; set; } = null!;

    public bool IsPrimaryCharacter { get; set; }
    public bool IsInCharacterCreate { get; set; }
    public bool IsRetired { get; set; }
    public int? ExpressionSubTypeId { get; set; }
}
