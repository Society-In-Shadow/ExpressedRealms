namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetArchetypesForExpression;

public class ArchetypeCharacterInfo
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Background { get; set; }
}
