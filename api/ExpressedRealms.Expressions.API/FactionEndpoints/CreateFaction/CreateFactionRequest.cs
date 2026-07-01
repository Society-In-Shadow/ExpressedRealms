namespace ExpressedRealms.Expressions.API.FactionEndpoints.CreateFaction;

public class CreateFactionRequest
{
    public required string Name { get; set; }
    public required string Background { get; set; }
    public required int ExpressionId { get; set; }
    public required string Specialization { get; set; }
    public int KnowledgeId { get; set; }
}
