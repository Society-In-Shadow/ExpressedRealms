namespace ExpressedRealms.Knowledges.UseCases.Knowledges.GetEditKnowledge;

public class GetEditKnowledgeReturnModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int KnowledgeTypeId { get; set; }
    public List<KnowledgeTypeModel> KnowledgeTypes { get; set; } = new();
}
