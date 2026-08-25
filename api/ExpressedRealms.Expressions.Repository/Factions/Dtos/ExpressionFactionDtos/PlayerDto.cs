namespace ExpressedRealms.Expressions.Repository.Factions.Dtos.ExpressionFactionDtos;

public record PlayerDto()
{
    public int Id { get; init; }
    public int Level { get; set; }
    public required string LevelName { get; set; }
    public required string CharacterName { get; init; }
    public required string Player { get; init; }
}