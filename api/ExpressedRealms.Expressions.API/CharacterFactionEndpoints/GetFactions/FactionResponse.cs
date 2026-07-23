namespace ExpressedRealms.Expressions.API.CharacterFactionEndpoints.GetFactions;

public class FactionResponse
{
    public List<FactionLevelModel> FactionLevels { get; set; } = new();
    public int? FactionId { get; set; }
}
