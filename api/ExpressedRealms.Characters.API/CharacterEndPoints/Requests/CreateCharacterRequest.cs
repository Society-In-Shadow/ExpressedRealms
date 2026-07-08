namespace ExpressedRealms.Characters.API.CharacterEndPoints.Requests;

internal record CreateCharacterRequest
{
    public string Name { get; set; } = null!;
    public int ExpressionId { get; set; }
    public bool IsArchetype { get; set; }
}
