using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup;

public class BlessingLevelConfiguration : IEntityTypeConfiguration<BlessingLevel>
{
    public void Configure(EntityTypeBuilder<BlessingLevel> builder)
    {
        builder.ToTable("blessing_level");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Level).HasColumnName("level").HasMaxLength(25).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").IsRequired();
        builder.Property(e => e.BlessingId).HasColumnName("blessing_id").IsRequired();
        builder.Property(e => e.XpCost).HasColumnName("xp_cost").IsRequired();
        builder.Property(e => e.XpGain).HasColumnName("xp_gain").IsRequired();

        builder
            .HasOne(x => x.Blessing)
            .WithMany(x => x.BlessingLevels)
            .HasForeignKey(x => x.BlessingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
