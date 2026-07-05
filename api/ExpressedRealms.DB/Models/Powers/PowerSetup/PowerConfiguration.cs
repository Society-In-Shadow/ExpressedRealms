using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerConfiguration : IEntityTypeConfiguration<Power>
{
    public void Configure(EntityTypeBuilder<Power> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired().HasMaxLength(20_000);
        builder.Property(e => e.LevelId).IsRequired();
        builder.Property(e => e.Cost).HasMaxLength(250);
        builder.Property(e => e.GameMechanicEffect).HasMaxLength(20_000);
        builder.Property(e => e.Limitation).HasMaxLength(20_000);
        builder.Property(e => e.OtherFields).HasMaxLength(20_000);
        builder.Property(e => e.StatModifierGroupId);
        builder.Property(e => e.CloneSourceId);
        builder.Property(e => e.CloneBatchId);

        builder
            .HasOne(e => e.CloneSource)
            .WithMany(e => e.CloneTargets)
            .HasForeignKey(e => e.CloneSourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.StatModifierGroup)
            .WithMany(e => e.Powers)
            .HasForeignKey(e => e.StatModifierGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasOne(e => e.PowerLevel)
            .WithMany(e => e.Powers)
            .HasForeignKey(e => e.LevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.PowerAreaOfEffectType)
            .WithMany(e => e.Powers)
            .HasForeignKey(e => e.AreaOfEffectTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.PowerActivationTimingType)
            .WithMany(e => e.Powers)
            .HasForeignKey(e => e.ActivationTimingTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.PowerDuration)
            .WithMany(e => e.Powers)
            .HasForeignKey(e => e.DurationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
