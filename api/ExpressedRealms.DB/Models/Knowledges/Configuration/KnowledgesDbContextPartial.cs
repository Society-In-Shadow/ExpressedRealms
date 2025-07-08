using ExpressedRealms.DB.Models.Knowledges;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Knowledge> Knowledges { get; set; }
    public DbSet<KnowledgeType> KnowledgeTypes { get; set; }
}
