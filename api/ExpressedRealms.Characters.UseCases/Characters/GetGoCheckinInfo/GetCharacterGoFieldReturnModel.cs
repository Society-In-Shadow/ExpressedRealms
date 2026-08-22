namespace ExpressedRealms.Characters.UseCases.Characters.GetGoCheckinInfo;

public class GetCharacterGoFieldReturnModel
{
    public List<KnowledgeToCheck> Knowledges { get; set; } = [];
    public List<ContactCheck> Contacts { get; set; } = [];
}
