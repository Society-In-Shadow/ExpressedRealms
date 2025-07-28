using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;
// ReSharper disable once CheckNamespace
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Blessing> Blessings { get; set; }
    public DbSet<BlessingAuditTrail> BlessingAuditTrails { get; set; }
    public DbSet<BlessingLevel> BlessingLevels { get; set; }
    public DbSet<BlessingLevelAuditTrail> BlessingLevelAuditTrails { get; set; }
}
