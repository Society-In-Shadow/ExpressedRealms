using ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetGoCheckinInfo;

public class GoCheckinChecks
{
    public List<KnowledgeCheck> KnowledgeChecks { get; set; } = new();
    public List<ContactCheck> Contacts { get; set; }
}
