using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup;
using ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;
using ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace

// ReSharper disable once CheckNamespace
namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Blessing> Blessings { get; set; }
    public DbSet<BlessingAuditTrail> BlessingAuditTrails { get; set; }
    public DbSet<BlessingLevel> BlessingLevels { get; set; }
    public DbSet<BlessingLevelAuditTrail> BlessingLevelAuditTrails { get; set; }
    public DbSet<CharacterBlessingMapping> CharacterBlessingMappings { get; set; }
}
