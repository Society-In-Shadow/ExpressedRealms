namespace ExpressedRealms.Expressions.Repository.CharacterFactions.Dtos;

public class PlayerFactionInfoDto
{
    public int FactionId { get; set; }
    public string FactionName { get; set; } = null!;
    public int FactionLevelId { get; set; }
    public string FactionRank { get; set; } = null!;
}