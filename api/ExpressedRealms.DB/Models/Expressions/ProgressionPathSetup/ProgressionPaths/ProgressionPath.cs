using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Characters;
using ExpressedRealms.DB.Models.Expressions.ExpressionSetup;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;
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

    public Expression Expression { get; set; } = null!;
    public virtual List<Character> PrimaryProgressions { get; set; } = null!;
    public virtual List<Character> SecondaryProgressions { get; set; } = null!;
    public List<ProgressionLevel> ProgressionLevels { get; set; } = null!;
    public List<ProgressionPathAuditTrail> ProgressionPathAuditTrails { get; set; } = null!;
    public List<ProgressionLevelAuditTrail> ProgressionLevelAuditTrails { get; set; } = null!;
}
