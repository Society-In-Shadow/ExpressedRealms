namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;

public class UpdateKnowledgeForCharacterModel
{
    public int MappingId { get; set; }
    public int KnowledgeLevelId { get; set; }
    public string? Notes { get; set; }
}
