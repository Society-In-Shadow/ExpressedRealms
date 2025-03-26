using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerActivationTimingTypeConfiguration
    : IEntityTypeConfiguration<PowerActivationTimingType>
{
    public void Configure(EntityTypeBuilder<PowerActivationTimingType> builder)
    {
        builder.ToTable("power_activation_timing_type");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();
    }
}
