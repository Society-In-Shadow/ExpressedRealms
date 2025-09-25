using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;

[AuditInclude]
public class ProgressionLevel : ISoftDelete
{
    public int Id { get; set; }
    public int ProgressionPathId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public List<ProgressionLevelAuditTrail> ProgressionLevelAuditTrails { get; set; } = null!;
}
