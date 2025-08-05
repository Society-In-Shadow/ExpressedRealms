namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.GetOptions;

public class KnowledgeOptionsResponse
{
    public List<KnowledgeOptions> KnowledgeLevels { get; set; } = new();
    public int AvailableExperience { get; set; }
}
