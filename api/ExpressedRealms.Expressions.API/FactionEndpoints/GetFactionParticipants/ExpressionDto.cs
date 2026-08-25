namespace ExpressedRealms.Expressions.API.FactionEndpoints.GetFactionParticipants;

public record ExpressionDto()
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public List<FactionDto> Factions { get; init; } = [];
};