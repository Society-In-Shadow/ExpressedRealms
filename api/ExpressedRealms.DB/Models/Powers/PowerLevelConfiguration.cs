using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerLevelConfiguration : IEntityTypeConfiguration<PowerLevel>
{
    public void Configure(EntityTypeBuilder<PowerLevel> builder)
    {
        builder.ToTable("power_level");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").IsRequired();
        builder.Property(e => e.Xp).HasColumnName("xp").IsRequired();
        builder.Property(e => e.TotalXp).HasColumnName("total_xp").IsRequired().HasDefaultValue(0);
    }
}
