namespace ExpressedRealms.Expressions.UseCases.FactionUseCases.GetFaction;

public class GetFactionReturnModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Background { get; set; }
}
