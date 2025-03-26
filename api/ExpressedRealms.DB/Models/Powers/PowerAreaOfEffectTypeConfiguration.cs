using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerAreaOfEffectTypeConfiguration : IEntityTypeConfiguration<PowerAreaOfEffectType>
{
    public void Configure(EntityTypeBuilder<PowerAreaOfEffectType> builder)
    {
        builder.ToTable("power_area_of_effect_type");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();
    }
}
