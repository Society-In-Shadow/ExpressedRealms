using ExpressedRealms.Knowledges.UseCases.KnowledgeTypes.GetKnowledgeTypes;

namespace ExpressedRealms.Knowledges.UseCases.KnowledgeLevels;

public class GetKnowledgeLevelsReturnModel
{
    public List<KnowledgeLevelModel> KnowledgeLevels { get; set; } = new();
    public int AvailableExperience { get; set; }
}
