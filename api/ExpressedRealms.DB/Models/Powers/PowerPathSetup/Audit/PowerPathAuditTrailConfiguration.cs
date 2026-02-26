using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerPathSetup;

internal class PowerPathAuditTrailConfiguration : IEntityTypeConfiguration<PowerPathAuditTrail>
{
    public void Configure(EntityTypeBuilder<PowerPathAuditTrail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.ExpressionId).IsRequired();
        builder.Property(e => e.PowerPathId).IsRequired();

        builder.Property(e => e.Action).IsRequired();
        builder.Property(e => e.ActorUserId).IsRequired();
        builder.Property(e => e.Timestamp).IsRequired();
        builder.Property(e => e.ChangedProperties).IsRequired();

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.PowerPathAudits)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.PowerPath)
            .WithMany(x => x.PowerPathAudits)
            .HasForeignKey(x => x.PowerPathId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ActorUser)
            .WithMany(x => x.PowerPathAudits)
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
