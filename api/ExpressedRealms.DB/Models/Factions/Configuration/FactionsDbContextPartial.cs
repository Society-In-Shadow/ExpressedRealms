using ExpressedRealms.DB.Models.Factions.FactionLevelModels;
using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.DB.Models.Factions.FactionModels.Audit;
using ExpressedRealms.DB.Models.Factions.FactionRankModels;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Faction> Factions { get; set; }
    public DbSet<FactionAuditTrail> FactionAuditTrails { get; set; }
    public DbSet<FactionLevel> FactionLevels { get; set; }
    public DbSet<FactionRank> FactionRanks { get; set; }
}
