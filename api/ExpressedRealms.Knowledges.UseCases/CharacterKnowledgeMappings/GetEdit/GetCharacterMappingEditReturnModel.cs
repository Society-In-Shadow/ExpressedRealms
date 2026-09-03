namespace ExpressedRealms.Knowledges.UseCases.CharacterKnowledgeMappings.GetEdit;

public class GetCharacterMappingEditReturnModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SelectedLevelId { get; set; }
    public List<KnowledgeLevel> KnowledgeLevels { get; set; } = [];
    public string? Notes { get; set; }
    public string KnowledgeType { get; set; } = string.Empty;
    public List<SpecializationReturnModel> Specializations { get; set; } = [];
    public int? MinimumKnowledgeId { get; set; }
    public bool BlockFactionChanges { get; set; }
}
