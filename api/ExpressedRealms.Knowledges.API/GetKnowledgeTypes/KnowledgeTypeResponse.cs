using ExpressedRealms.Powers.API.PowerEndpoints.GetPowerOptions;

namespace ExpressedRealms.Knowledges.API.GetKnowledgeTypes;

public class KnowledgeTypeResponse
{
    public List<DetailedEditInformation> KnowledgeTypes { get; set; } = new();
}
