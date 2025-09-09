namespace ExpressedRealms.Characters.API.CharacterEndPoints.DTOs;

internal class CharacterOptionExpression
{
    public int Id { get; set; }

    /// <example>Adept</example>
    public required string Name { get; set; }
}
