namespace ExpressedRealms.Characters.Repository.DTOs;

public sealed record ArchetypeDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string ExpressionName { get; set; }
}
