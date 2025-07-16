using ExpressedRealms.DB.Models.Knowledges;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Knowledge> Knowledges { get; set; }
    public DbSet<KnowledgeAuditTrail> KnowledgeAuditTrails { get; set; }
    public DbSet<KnowledgeType> KnowledgeTypes { get; set; }
}
