namespace ExpressedRealms.Knowledges.UseCases.Knowledges.EditKnowledge;

public class EditKnowledgeModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int KnowledgeTypeId { get; set; }
}
