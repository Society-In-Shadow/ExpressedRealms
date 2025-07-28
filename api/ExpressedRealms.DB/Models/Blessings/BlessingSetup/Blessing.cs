using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;

namespace ExpressedRealms.DB.Models.Blessings.BlessingSetup;

[AuditInclude]
public class Blessing : ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? SubCategory { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<BlessingAuditTrail> BlessingAuditTrails { get; set; } = null!;
    public virtual List<BlessingLevelAuditTrail> BlessingLevelAuditTrails { get; set; } = null!;
}
