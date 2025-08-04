using ExpressedRealms.Knowledges.API.GetAllExpressions;

namespace ExpressedRealms.Knowledges.API.CharacterKnowledges.GetAll;

public class CharacterKnowledgeBaseResponse
{
    public List<CharacterKnowledgeResponse> Knowledges { get; set; } = new();
}