using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.DB.Models.Knowledges.Configuration;

public static class KnowledgesConfiguration
{
    public static void AddKnowledgeConfiguration(this ModelBuilder builder)
    {
        builder.ApplyConfiguration(new KnowledgeConfiguration());
        builder.ApplyConfiguration(new KnowledgeTypeConfiguration());
        builder.ApplyConfiguration(new KnowledgeAuditTrailConfiguration());
    }
}
