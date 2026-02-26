using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;

public class BlessingLevelConfiguration : IEntityTypeConfiguration<BlessingLevel>
{
    public void Configure(EntityTypeBuilder<BlessingLevel> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Level).HasMaxLength(25).IsRequired();
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.BlessingId).IsRequired();
        builder.Property(e => e.XpCost).IsRequired();
        builder.Property(e => e.XpGain).IsRequired();

        builder.Property(e => e.StatModifierGroupId);
        builder
            .HasOne(e => e.StatModifierGroup)
            .WithMany(e => e.BlessingLevels)
            .HasForeignKey(e => e.StatModifierGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Blessing)
            .WithMany(x => x.BlessingLevels)
            .HasForeignKey(x => x.BlessingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
