namespace ExpressedRealms.Characters.Repository.DTOs;

public sealed record ArchetypeCharacterInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
}
