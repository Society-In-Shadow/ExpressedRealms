namespace ExpressedRealms.Characters.Repository.DTOs;

public sealed record AddCharacterDto
{
    public string Name { get; init; } = null!;
    public int ExpressionId { get; init; }
    public bool IsArchetype { get; set; }
}
