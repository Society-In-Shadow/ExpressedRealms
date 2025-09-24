using ExpressedRealms.Characters.Repository.DTOs;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.Responses;

internal record CharacterEditResponse
{
    public CharacterEditResponse(GetEditCharacterDto dto)
    {
        Name = dto.Name;
        Background = dto.Background;
        Expression = dto.Expression;
        FactionId = dto.FactionId;
        ExpressionId = dto.ExpressionId;
        IsPrimaryCharacter = dto.IsPrimaryCharacter;
        IsInCharacterCreation = dto.IsInCharacterCreation;
        IsOwner = dto.IsOwner;
    }

    /// <summary>
    /// Indicates if the user is the owner of the character.
    /// </summary>
    public bool IsOwner { get; set; }

    public bool IsInCharacterCreation { get; set; }

    public bool IsPrimaryCharacter { get; set; }

    public int ExpressionId { get; set; }

    /// <example>John Doe</example>
    public string Name { get; set; } = null!;

    /// <example>John Doe is a high elf from the northern woods.</example>
    public string? Background { get; set; }

    /// <example>Adept</example>
    public string Expression { get; set; }

    /// <example>8</example>
    public int? FactionId { get; set; }
}
