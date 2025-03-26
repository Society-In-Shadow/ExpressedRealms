using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerLevelConfiguration : IEntityTypeConfiguration<PowerLevel>
{
    public void Configure(EntityTypeBuilder<PowerLevel> builder)
    {
        builder.ToTable("power_level");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();
        builder.Property(e => e.Xp).IsRequired();
    }
}
