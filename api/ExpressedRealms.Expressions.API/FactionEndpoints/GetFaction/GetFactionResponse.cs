namespace ExpressedRealms.Expressions.API.FactionEndpoints.GetFaction;

public class GetFactionResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Background { get; set; }
}
