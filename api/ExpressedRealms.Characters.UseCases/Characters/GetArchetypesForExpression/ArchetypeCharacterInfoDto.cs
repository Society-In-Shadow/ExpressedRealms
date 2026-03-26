namespace ExpressedRealms.Characters.UseCases.Characters.GetArchetypesForExpression;

public sealed record ArchetypeCharacterInfoDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Background { get; set; }
}
