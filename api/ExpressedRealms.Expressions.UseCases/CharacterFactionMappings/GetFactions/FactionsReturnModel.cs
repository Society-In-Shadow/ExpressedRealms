namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMappings.GetFactions;

public class FactionsReturnModel
{
    public List<CharacterFactionLevelInfo> FactionLevels { get; set; } = new();
    public int? FactionId { get; set; }
}
