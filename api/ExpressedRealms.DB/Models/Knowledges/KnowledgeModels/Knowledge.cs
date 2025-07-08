using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;

namespace ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

public class Knowledge : ISoftDelete
{
    public int Id { get; set; }
    public int KnowledgeTypeId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual KnowledgeType KnowledgeType { get; set; } = null!;
    public virtual List<KnowledgeAuditTrail> KnowledgeAuditTrails { get; set; } = null!;
}
