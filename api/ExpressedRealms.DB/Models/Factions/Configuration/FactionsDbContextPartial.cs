using ExpressedRealms.DB.Models.Factions.FactionModels;
using ExpressedRealms.DB.Models.Factions.FactionModels.Audit;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Faction> Factions { get; set; }
    public DbSet<FactionAuditTrail> FactionAuditTrails { get; set; }
}
