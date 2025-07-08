namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetKnowledges;

public class KnowledgeModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TypeName { get; set; }
    public string TypeDescription { get; set; }
    public int TypeId { get; set; }
}