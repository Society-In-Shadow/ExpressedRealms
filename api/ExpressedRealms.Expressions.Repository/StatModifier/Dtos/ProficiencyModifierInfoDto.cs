namespace ExpressedRealms.Characters.Repository.Proficiencies.DTOs;

public class ProficiencyModifierInfoDto
{
    public required string Source { get; set; }
    public int Modifier { get; set; }
    public int ModifierTypeId { get; set; }
    public bool ScaleWithLevel { get; set; }
    public bool CreationSpecificBonus { get; set; }
}