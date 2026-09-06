namespace ExpressedRealms.Characters.Repository.DTOs;

public record CharacterDiffIdsDto()
{
    public int NewestCharacterId { get; init; }
    public int PreviousCharacterId { get; set; }
};
