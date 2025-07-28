using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;

namespace ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;

[AuditInclude]
public class BlessingLevel : ISoftDelete
{
    public int Id { get; set; }
    public int BlessingId { get; set; }
    public string Level { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int XpCost { get; set; }
    public int XpGain { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual Blessing Blessing { get; set; } = null!;
    public virtual List<BlessingLevelAuditTrail> BlessingLevelAuditTrails { get; set; } = null!;
}
