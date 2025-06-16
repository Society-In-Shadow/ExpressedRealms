using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerSetup.Audit;

internal class PowerAuditTrailConfiguration : IEntityTypeConfiguration<PowerAuditTrail>
{
    public void Configure(EntityTypeBuilder<PowerAuditTrail> builder)
    {
        builder.ToTable("power_audit_trail");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.PowerId).HasColumnName("power_id").IsRequired();
        builder.Property(e => e.PowerPathId).HasColumnName("power_path_id").IsRequired();

        builder.Property(e => e.Action).HasColumnName("action").IsRequired();
        builder.Property(e => e.ActorUserId).HasColumnName("actor_user_id").IsRequired();
        builder.Property(e => e.Timestamp).HasColumnName("timestamp").IsRequired();
        builder.Property(e => e.ChangedProperties).HasColumnName("changed_properties").IsRequired();

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
