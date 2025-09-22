using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Characters.xpTables;

public class CharacterXpViewConfiguration : IEntityTypeConfiguration<CharacterXpView>
{
    public void Configure(EntityTypeBuilder<CharacterXpView> builder)
    {
        builder.HasNoKey();
        builder.ToView("character_xp_view"); // maps to the DB view (read-only)

        builder.Property(p => p.CharacterId).HasColumnName("character_id");
        builder.Property(p => p.SectionTypeId).HasColumnName("section_type_id");
        builder.Property(p => p.SectionName).HasColumnName("section_name");
        builder.Property(p => p.SectionCap).HasColumnName("section_cap");
        builder.Property(p => p.TrueSectionCap).HasColumnName("true_section_cap");
        builder.Property(p => p.SpentXp).HasColumnName("spent_xp");
        builder.Property(p => p.TrueTotalSpent).HasColumnName("true_total_spent");
        builder.Property(p => p.DiscretionXp).HasColumnName("discretion_xp");
        builder
            .Property(p => p.TotalCharacterCreationXp)
            .HasColumnName("total_character_creation_xp");
        builder.Property(p => p.LevelXp).HasColumnName("level_xp");
    }
}
