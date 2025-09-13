using Audit.EntityFramework;
using ExpressedRealms.DB.Interceptors;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;

namespace ExpressedRealms.DB.Models.Blessings.BlessingSetup;

[AuditInclude]
public class Blessing : ISoftDelete
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string Type { get; set; } = null!;
    public required string SubCategory { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    public virtual List<BlessingAuditTrail> BlessingAuditTrails { get; set; } = null!;
    public virtual List<BlessingLevel> BlessingLevels { get; set; } = null!;
    public virtual List<BlessingLevelAuditTrail> BlessingLevelAuditTrails { get; set; } = null!;
    public virtual List<CharacterBlessingMapping> CharacterBlessingMappings { get; set; } = null!;
}
