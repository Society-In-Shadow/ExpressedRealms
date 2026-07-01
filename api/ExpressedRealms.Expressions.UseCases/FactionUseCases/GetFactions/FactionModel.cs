namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFactions;

public class FactionModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Background { get; set; }
    public List<FactionLevelModel> FactionLevels { get; set; } = new();
}
