namespace ExpressedRealms.Characters.API.CharacterEndPoints.CopyCharacter;

internal record CopyCharacterRequest
{
    public required string CharacterName { get; set; }
}
