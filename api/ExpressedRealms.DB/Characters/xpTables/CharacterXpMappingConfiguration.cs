using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Characters.xpTables;

public class CharacterXpMappingConfiguration : IEntityTypeConfiguration<CharacterXpMapping>
{
    public void Configure(EntityTypeBuilder<CharacterXpMapping> builder)
    {
        builder.ToTable("character_xp_mapping");

        builder.HasKey(x => new { x.CharacterId, x.XpSectionTypeId });
        builder.Property(x => x.CharacterId).HasColumnName("character_id").IsRequired();
        builder.Property(x => x.SectionCap).HasColumnName("section_cap").IsRequired();
        builder.Property(x => x.XpSectionTypeId).HasColumnName("xp_section_type_id").IsRequired();
        builder.Property(x => x.DiscretionXp).HasColumnName("discretion_xp").IsRequired();
        builder.Property(x => x.SpentXp).HasColumnName("spent_xp").IsRequired();
        builder
            .Property(x => x.TotalCharacterCreationXp)
            .HasColumnName("total_character_creation_xp")
            .IsRequired();
        builder.Property(x => x.LevelXp).HasColumnName("level_xp").IsRequired();

        builder
            .HasOne(x => x.Character)
            .WithMany(x => x.CharacterXpMappings)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.XpSectionType)
            .WithMany(x => x.CharacterXpMappings)
            .HasForeignKey(x => x.XpSectionTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
