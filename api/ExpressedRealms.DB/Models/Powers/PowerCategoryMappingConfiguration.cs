using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers;

public class PowerCategoryMappingConfiguration : IEntityTypeConfiguration<PowerCategoryMapping>
{
    public void Configure(EntityTypeBuilder<PowerCategoryMapping> builder)
    {
        builder.ToTable("power_category_mapping");

        builder.HasKey(e => new { e.PowerId, e.CategoryId });
        builder.Property(e => e.PowerId).IsRequired();
        builder.Property(e => e.CategoryId).IsRequired();

        builder
            .HasOne(e => e.Power)
            .WithMany(e => e.CategoryMappings)
            .HasForeignKey(e => e.PowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Category)
            .WithMany(e => e.PowerMappings)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
