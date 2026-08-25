namespace ExpressedRealms.Expressions.Repository.Factions.Dtos.ExpressionFactionDtos;

public record ExpressionFactionDto()
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public List<PlayerDto> Players { get; init; } = [];
};
