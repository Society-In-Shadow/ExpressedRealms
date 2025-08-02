namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Create;

public class AddModel
{
    public int KnowledgeLevelId { get; set; }
    public int CharacterId { get; set; }
    public int KnowledgeId { get; set; }
    public string? Notes { get; set; }
}
