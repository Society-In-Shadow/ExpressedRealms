namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledges;

public class KnowledgeModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string TypeName { get; set; }
    public required string TypeDescription { get; set; }
    public int TypeId { get; set; }
}
