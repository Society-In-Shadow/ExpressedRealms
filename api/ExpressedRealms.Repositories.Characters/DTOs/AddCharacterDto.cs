namespace ExpressedRealms.Repositories.Characters.DTOs;

public sealed record AddCharacterDto
{
    public string Name { get; init; } = null!;
    public string? Background { get; init; }
    public int ExpressionId { get; init; }
    public int FactionId { get; init; }
}
