namespace ExpressedRealms.Characters.Repository.DTOs;

public record CharacterGoInformationProjection()
{
    public int Id { get; init; }
    public bool IsInCharacterCreation { get; init; }
    public bool ExpressionIsLegacy { get; init; }
};