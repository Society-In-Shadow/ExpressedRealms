using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerCategoryConfiguration : IEntityTypeConfiguration<PowerCategory>
{
    public void Configure(EntityTypeBuilder<PowerCategory> builder)
    {
        builder.ToTable("power_category");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).IsRequired();
    }
}
