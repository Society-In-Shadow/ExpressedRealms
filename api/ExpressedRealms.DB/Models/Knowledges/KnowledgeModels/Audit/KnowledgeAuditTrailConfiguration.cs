using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.KnowledgeModels.Audit;

internal class KnowledgeAuditTrailConfiguration : IEntityTypeConfiguration<KnowledgeAuditTrail>
{
    public void Configure(EntityTypeBuilder<KnowledgeAuditTrail> builder)
    {
        builder.ToTable("knowledges_audit_trail");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.KnowledgeId).HasColumnName("knowledge_id").IsRequired();

        builder.Property(e => e.Action).HasColumnName("action").IsRequired();
        builder.Property(e => e.ActorUserId).HasColumnName("actor_user_id").IsRequired();
        builder.Property(e => e.Timestamp).HasColumnName("timestamp").IsRequired();
        builder.Property(e => e.ChangedProperties).HasColumnName("changed_properties").IsRequired();

        builder
            .HasOne(x => x.Knowledge)
            .WithMany(x => x.KnowledgeAuditTrails)
            .HasForeignKey(x => x.KnowledgeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ActorUser)
            .WithMany(x => x.KnowledgeAuditTrails)
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
