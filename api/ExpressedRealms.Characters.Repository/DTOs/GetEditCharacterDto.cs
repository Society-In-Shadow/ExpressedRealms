namespace ExpressedRealms.Characters.Repository.DTOs;

public sealed record GetEditCharacterDto
{
    public string Name { get; init; } = null!;
    public string? Background { get; init; }
    public string Expression { get; init; } = null!;
    public int? FactionId { get; init; }
}
