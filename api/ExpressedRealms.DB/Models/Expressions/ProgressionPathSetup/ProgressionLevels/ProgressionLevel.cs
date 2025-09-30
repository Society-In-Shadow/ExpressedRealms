using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;
using ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;
using ExpressedRealms.DB.Models.ModifierSystem.StatModifierGroups;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;

[AuditInclude]
public class ProgressionLevel : ISoftDelete
{
    public int Id { get; set; }
    public int ProgressionPathId { get; set; }
    public int XlLevel { get; set; }
    public string Description { get; set; } = null!;

    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public int StatModifierGroupId { get; set; }
    public StatModifierGroup StatModifierGroup { get; set; }

    public ProgressionPath ProgressionPath { get; set; } = null!;
    public List<ProgressionLevelAuditTrail> ProgressionLevelAuditTrails { get; set; } = null!;
}
