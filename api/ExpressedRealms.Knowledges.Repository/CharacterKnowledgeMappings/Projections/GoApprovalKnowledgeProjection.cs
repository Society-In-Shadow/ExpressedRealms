namespace ExpressedRealms.Knowledges.Repository.CharacterKnowledgeMappings.Projections;

public class GoApprovalKnowledgeProjection
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int LevelId { get; set; }
    public int KnowledgeTypeId { get; set; }
}