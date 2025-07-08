using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

namespace ExpressedRealms.DB.Models.Knowledges;

public class KnowledgeType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public virtual List<Knowledge> Knowledges { get; set; } = null!;
}
