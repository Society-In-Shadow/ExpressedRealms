using ExpressedRealms.Characters.Repository.DTOs;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.Responses;

internal record CharacterEditResponse
{
    public CharacterEditResponse(GetEditCharacterDto dto)
    {
        Name = dto.Name;
        Background = dto.Background;
        Expression = dto.Expression;
        ExpressionSubTypeId = dto.ExpressionSubTypeId;
        ExpressionId = dto.ExpressionId;
        IsPrimaryCharacter = dto.IsPrimaryCharacter;
        IsInCharacterCreation = dto.IsInCharacterCreation;
        IsOwner = dto.IsOwner;
        PrimaryProgressionId = dto.PrimaryProgressionId;
        SecondaryProgressionId = dto.SecondaryProgressionId;
        IsRetired = dto.IsRetired;
        IsArchetypeCharacter = dto.IsArchetypeCharacter;
    }

    public bool IsArchetypeCharacter { get; set; }

    public bool IsRetired { get; set; }

    public int? SecondaryProgressionId { get; set; }

    public int? PrimaryProgressionId { get; set; }

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

    public int? ExpressionSubTypeId { get; set; }
}
