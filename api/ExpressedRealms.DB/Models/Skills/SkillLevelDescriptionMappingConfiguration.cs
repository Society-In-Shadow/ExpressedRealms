using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class SkillLevelDescriptionMappingConfiguration
    : IEntityTypeConfiguration<SkillLevelDescriptionMapping>
{
    public void Configure(EntityTypeBuilder<SkillLevelDescriptionMapping> builder)
    {
        builder.ToTable(nameof(SkillLevelDescriptionMapping));

        builder.HasKey(e => new { e.SkillLevelId, e.SkillTypeId });

        builder.Property(e => e.SkillTypeId).IsRequired();
        builder.Property(e => e.SkillLevelId).IsRequired();
        builder.Property(e => e.Description).IsRequired().HasDefaultValue("Everything is awesome!");

        builder
            .HasOne(e => e.SkillLevel)
            .WithMany(e => e.CharacterLevelDescriptions)
            .HasForeignKey(e => e.SkillLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.SkillType)
            .WithMany(e => e.CharacterLevelDescriptions)
            .HasForeignKey(e => e.SkillTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
