namespace ExpressedRealms.Expressions.UseCases.CharacterFactionMapping.GetFactions;

public class FactionsReturnModel
{
    public List<CharacterFactionLevelInfo> FactionLevels { get; set; } = new();
}
