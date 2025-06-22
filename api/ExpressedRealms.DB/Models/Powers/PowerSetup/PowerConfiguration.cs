using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerConfiguration : IEntityTypeConfiguration<Power>
{
    public void Configure(EntityTypeBuilder<Power> builder)
    {
        builder.ToTable("power");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.LevelId).IsRequired();
        builder.Property(e => e.PowerPathId).HasColumnName("power_path_id").IsRequired();
        builder.Property(e => e.Cost).HasColumnName("cost");

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

        builder
            .HasOne(e => e.PowerPath)
            .WithMany(e => e.Powers)
            .HasForeignKey(e => e.PowerPathId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
