namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetAllFactionParticipants;

public record ExpressionDto()
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public List<FactionDto> Factions { get; init; } = [];
};
