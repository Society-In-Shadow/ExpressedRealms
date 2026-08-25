namespace ExpressedRealms.Expressions.Repository.Factions.Dtos.ExpressionFactionDtos;

public record ExpressionDto()
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public List<ExpressionFactionDto> Factions { get; init; } = [];
};
