namespace ExpressedRealms.Characters.Repository.DTOs;

public sealed record GetEditCharacterDto
{
    public string Name { get; init; } = null!;
    public string? Background { get; init; }
    public string Expression { get; init; } = null!;
    public int? FactionId { get; init; }
    public int ExpressionId { get; set; }
    public bool IsPrimaryCharacter { get; set; }
    public bool IsInCharacterCreation { get; set; }
    public bool IsOwner { get; set; }
    public int? PrimaryProgressionId { get; set; }
    public int? SecondaryProgressionId { get; set; }
    public bool IsRetired { get; set; }
}
