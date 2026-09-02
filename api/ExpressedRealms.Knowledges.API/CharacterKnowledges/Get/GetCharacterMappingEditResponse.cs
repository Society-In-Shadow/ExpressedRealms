namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.Get;

public class GetCharacterMappingEditResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int SelectedLevelId { get; set; }
    public List<KnowledgeLevel> KnowledgeLevels { get; set; } = [];
}
