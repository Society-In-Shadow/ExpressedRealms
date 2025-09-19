namespace ExpressedRealms.Characters.Repository.DTOs;

public class CharacterStatusDto
{
    public bool IsPrimaryCharacter { get; set; }
    public bool IsInCharacterCreation { get; set; }
    public int AssignedXp { get; set; }
}