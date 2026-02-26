using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;

public class ProgressionLevelConfiguration : IEntityTypeConfiguration<ProgressionLevel>
{
    public void Configure(EntityTypeBuilder<ProgressionLevel> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.ProgressionPathId).IsRequired();
        builder.Property(e => e.XlLevel).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(5000).IsRequired();

        builder.Property(e => e.StatModifierGroupId);
        builder
            .HasOne(e => e.StatModifierGroup)
            .WithMany(e => e.ProgressionLevels)
            .HasForeignKey(e => e.StatModifierGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ProgressionPath)
            .WithMany(x => x.ProgressionLevels)
            .HasForeignKey(x => x.ProgressionPathId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
