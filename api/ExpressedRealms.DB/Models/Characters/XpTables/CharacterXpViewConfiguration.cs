using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.XpTables;

public class CharacterXpViewConfiguration : IEntityTypeConfiguration<CharacterXpView>
{
    public void Configure(EntityTypeBuilder<CharacterXpView> builder)
    {
        builder.HasNoKey();
        builder.ToView("character_xp_view"); // maps to the DB view (read-only)

        builder.Property(p => p.CharacterId);
        builder.Property(p => p.SectionTypeId);
        builder.Property(p => p.SectionName);
        builder.Property(p => p.SectionCap);
        builder.Property(p => p.TrueSectionCap);
        builder.Property(p => p.SpentXp);
        builder.Property(p => p.TrueTotalSpent);
        builder.Property(p => p.DiscretionXp);
        builder.Property(p => p.TotalCharacterCreationXp);
        builder.Property(p => p.LevelXp);
    }
}
