namespace ExpressedRealms.Expressions.API.FactionEndpoints.GetFactionParticipants;

public record FactionDto()
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public List<PlayerDto> Players { get; init; } = [];
};