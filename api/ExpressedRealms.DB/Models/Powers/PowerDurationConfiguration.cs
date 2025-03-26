using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerDurationConfiguration : IEntityTypeConfiguration<PowerDuration>
{
    public void Configure(EntityTypeBuilder<PowerDuration> builder)
    {
        builder.ToTable("power_duration");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();
    }
}
