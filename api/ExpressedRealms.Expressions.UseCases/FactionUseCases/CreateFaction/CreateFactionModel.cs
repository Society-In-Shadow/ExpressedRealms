namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.CreateFaction;

public class CreateFactionModel
{
    public required string Name { get; set; }
    public required string Background { get; set; }
    public required int ExpressionId { get; set; }
}
