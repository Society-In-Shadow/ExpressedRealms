using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSetup;

internal class ExpressionAuditTrailConfiguration : IEntityTypeConfiguration<ExpressionAuditTrail>
{
    public void Configure(EntityTypeBuilder<ExpressionAuditTrail> builder)
    {
        builder.ToTable("Expression_AuditTrail");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.ExpressionId).IsRequired();

        builder.Property(e => e.Action).IsRequired();
        builder.Property(e => e.ActorUserId).IsRequired();
        builder.Property(e => e.Timestamp).IsRequired();
        builder.Property(e => e.ChangedProperties).IsRequired();

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.ExpressionAudits)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ActorUser)
            .WithMany(x => x.ExpressionAuditTrails)
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
