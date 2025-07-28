using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;

internal class KnowledgeAuditTrailConfiguration : IEntityTypeConfiguration<KnowledgeAuditTrail>
{
    public void Configure(EntityTypeBuilder<KnowledgeAuditTrail> builder)
    {
        builder.ToTable("knowledges_audit_trail");

        builder.Property(e => e.KnowledgeId).HasColumnName("knowledge_id").IsRequired();

        builder
            .HasOne(x => x.Knowledge)
            .WithMany(x => x.KnowledgeAuditTrails)
            .HasForeignKey(x => x.KnowledgeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.KnowledgeAuditTrails);
    }
}
