namespace ExpressedRealms.Characters.API.CharacterEndPoints.Responses;

internal class HighLevelExpressionInfoResponse
{   
    public required string Name { get; set; }
    public required string? Background { get; set; }
    public required string Description { get; set; }
    public required string Archetypes { get; set; }
}
