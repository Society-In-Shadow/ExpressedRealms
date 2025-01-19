using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class CharacterSkillsMappingConfiguration : IEntityTypeConfiguration<CharacterSkillsMapping>
{
    public void Configure(EntityTypeBuilder<CharacterSkillsMapping> builder)
    {
        builder.ToTable(nameof(CharacterSkillsMapping));

        builder.HasKey(e => new
        {
            e.CharacterId,
            e.SkillLevelId,
            e.SkillTypeId,
        });

        builder.Property(e => e.SkillTypeId).IsRequired();
        builder.Property(e => e.SkillLevelId).IsRequired();
        builder.Property(e => e.CharacterId).IsRequired();

        builder
            .HasOne(e => e.Character)
            .WithMany(e => e.CharacterSkillsMappings)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.SkillLevel)
            .WithMany(e => e.CharacterSkillsMappings)
            .HasForeignKey(e => e.SkillLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.SkillType)
            .WithMany(e => e.CharacterSkillsMappings)
            .HasForeignKey(e => e.SkillTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
