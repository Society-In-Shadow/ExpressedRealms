using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths.Audit;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;

public class ProgressionPath : ISoftDelete
{
    public int Id { get; set; }
    public int ExpressionId { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public List<ProgressionPathAuditTrail> ProgressionPathAuditTrails { get; set; } = null!;
    public List<ProgressionLevelAuditTrail> ProgressionLevelAuditTrails { get; set; } = null!;
}
