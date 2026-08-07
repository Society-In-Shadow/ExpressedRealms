namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.Edit;

public class UpdateKnowledgeForCharacterModel
{
    public int CharacterId { get; set; }
    public int MappingId { get; set; }
    public int KnowledgeLevelId { get; set; }
    public string? Notes { get; set; }
}
