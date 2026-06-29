using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Factions.FactionLevelModels.Audit;
using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

namespace ExpressedRealms.DB.Models.Factions.FactionLevelModels;

[AuditInclude]
public class FactionLevel : ISoftDelete
{
    public int Id { get; set; }
    public int FactionId { get; set; }
    public int FactionRankId { get; set; }
    public int? KnowledgeId { get; set; }
    public int? KnowledgeLevelId { get; set; }
    public string? Specialization { get; set; }

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Faction Faction { get; set; } = null!;
    public virtual FactionRank FactionRank { get; set; } = null!;
    public virtual Knowledge? Knowledge { get; set; }
    public virtual KnowledgeEducationLevel? KnowledgeLevel { get; set; }
    public virtual ICollection<FactionLevelAuditTrail> FactionLevelAuditTrails { get; set; } =
        new HashSet<FactionLevelAuditTrail>();
}
