using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Statistics;

public class StatDescriptionMappingConfiguration : IEntityTypeConfiguration<StatDescriptionMapping>
{
    public void Configure(EntityTypeBuilder<StatDescriptionMapping> builder)
    {
        builder.HasKey(x => new { x.StatTypeId, x.StatLevelId });

        builder.Property(x => x.StatTypeId).IsRequired();
        builder.Property(x => x.StatLevelId).IsRequired();
        builder.Property(x => x.ReasonableExpectation).IsRequired().HasMaxLength(400);

        builder
            .HasOne(x => x.StatType)
            .WithMany(x => x.StatDescriptionMappings)
            .HasForeignKey(x => x.StatTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.StatLevel)
            .WithMany(x => x.StatDescriptionMappings)
            .HasForeignKey(x => x.StatLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
