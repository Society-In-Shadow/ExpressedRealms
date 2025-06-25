namespace ExpressedRealms.Characters.API.CharacterEndPoints.DTOs;

internal class CharacterOptionExpression
{
    public int Id { get; set; }

    /// <example>Adept</example>
    public string Name { get; set; }

    /// <example>Strong Martial Artists</example>
    public string ShortDescription { get; set; }
}
