namespace ExpressedRealms.Knowledges.API.GetAllExpressions;

public class KnowledgeViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; } = null!;
    public required string Description { get; set; }
    public required string TypeName { get; set; }
    public required string TypeDescription { get; set; }
    public int TypeId { get; set; }
}
