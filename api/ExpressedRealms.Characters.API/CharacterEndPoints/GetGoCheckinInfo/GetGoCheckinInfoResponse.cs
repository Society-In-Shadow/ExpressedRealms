namespace ExpressedRealms.Characters.API.CharacterEndPoints.GetGoCheckinInfo;

public class GoCheckinChecks
{
    public List<KnowledgeCheck> KnowledgeChecks { get; set; } = [];
    public List<ContactCheck> Contacts { get; set; } = [];
    public bool StillInCharacterCreation { get; set; }
    public bool SpentTooMuchXp { get; set; }
}
