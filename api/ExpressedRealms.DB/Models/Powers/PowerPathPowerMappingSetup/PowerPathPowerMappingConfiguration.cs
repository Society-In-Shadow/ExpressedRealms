using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.PowerPathPowerMappingSetup;

public class PowerPathPowerMappingConfiguration : IEntityTypeConfiguration<PowerPathPowerMapping>
{
    public void Configure(EntityTypeBuilder<PowerPathPowerMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.PowerPathId).IsRequired();
        builder.Property(e => e.PowerId).IsRequired();
        builder.Property(e => e.OrderIndex).IsRequired();

        builder
            .HasOne(e => e.PowerPath)
            .WithMany(e => e.PowerPathPowerMappings)
            .HasForeignKey(e => e.PowerPathId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
