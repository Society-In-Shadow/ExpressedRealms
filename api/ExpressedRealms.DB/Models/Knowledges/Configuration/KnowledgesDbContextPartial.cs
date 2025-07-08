using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<Knowledge> Knowledges { get; set; }
}
