using ExpressedRealms.Knowledges.UseCases.KnowledgeTypes.GetKnowledgeTypes;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;

public class GetKnowledgeLevelsModel
{
    public List<KnowledgeLevelModel> KnowledgeLevels { get; set; } = new();
}
