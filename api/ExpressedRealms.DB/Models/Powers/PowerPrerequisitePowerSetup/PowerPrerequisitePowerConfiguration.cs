using ExpressedRealms.DB.Models.Powers.PowerPrerequisiteSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerPrerequisitePowerSetup;

public class PowerPrerequisitePowerConfiguration : IEntityTypeConfiguration<PowerPrerequisitePower>
{
    public void Configure(EntityTypeBuilder<PowerPrerequisitePower> builder)
    {
        builder.ToTable("power_prerequisite_power");

        builder.HasKey(e => new { e.PrerequisiteId, e.PowerId });
        builder.Property(e => e.PrerequisiteId).HasColumnName("prerequisite_id").IsRequired();

        builder.Property(e => e.PowerId).HasColumnName("power_id").IsRequired();

        builder
            .HasOne(e => e.Power)
            .WithMany(e => e.PrerequisitePowers)
            .HasForeignKey(x => x.PowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Prerequisite)
            .WithMany(e => e.PrerequisitePowers)
            .HasForeignKey(x => x.PrerequisiteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
