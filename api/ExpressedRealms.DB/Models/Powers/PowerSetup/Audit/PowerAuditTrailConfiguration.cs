using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;

internal class PowerAuditTrailConfiguration : IEntityTypeConfiguration<PowerAuditTrail>
{
    public void Configure(EntityTypeBuilder<PowerAuditTrail> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.PowerId).IsRequired();
        builder.Property(e => e.PowerPathId).IsRequired();

        builder.Property(e => e.Action).IsRequired();
        builder.Property(e => e.ActorUserId).IsRequired();
        builder.Property(e => e.Timestamp).IsRequired();
        builder.Property(e => e.ChangedProperties).IsRequired();

        builder
            .HasOne(x => x.Power)
            .WithMany(x => x.PowerAuditTrails)
            .HasForeignKey(x => x.PowerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.PowerPath)
            .WithMany(x => x.PowerAuditTrails)
            .HasForeignKey(x => x.PowerPathId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ActorUser)
            .WithMany(x => x.PowerAuditTrails)
            .HasForeignKey(x => x.ActorUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
