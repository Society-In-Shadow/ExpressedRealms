using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class SkillLevelBenefitConfiguration : IEntityTypeConfiguration<SkillLevelBenefit>
{
    public void Configure(EntityTypeBuilder<SkillLevelBenefit> builder)
    {
        builder.ToTable(nameof(SkillLevelBenefit));

        builder.HasKey(e => new
        {
            e.SkillLevelId,
            e.SkillTypeId,
            e.ModifierTypeId
        });

        builder.Property(e => e.SkillTypeId).IsRequired();
        builder.Property(e => e.SkillLevelId).IsRequired();
        builder.Property(e => e.Modifier).IsRequired();
        builder.Property(e => e.ModifierTypeId).IsRequired();

        builder
            .HasOne(e => e.SkillLevel)
            .WithMany(e => e.SkillLevelBenefits)
            .HasForeignKey(e => e.SkillLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.SkillType)
            .WithMany(e => e.SkillLevelBenefits)
            .HasForeignKey(e => e.SkillTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.ModifierType)
            .WithMany(e => e.SkillLevelBenefits)
            .HasForeignKey(e => e.ModifierTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
