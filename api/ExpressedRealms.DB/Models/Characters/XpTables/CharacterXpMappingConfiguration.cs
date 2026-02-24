using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.XpTables;

public class CharacterXpMappingConfiguration : IEntityTypeConfiguration<CharacterXpMapping>
{
    public void Configure(EntityTypeBuilder<CharacterXpMapping> builder)
    {
        builder.HasKey(x => new { x.CharacterId, x.XpSectionTypeId });
        builder.Property(x => x.CharacterId).IsRequired();
        builder.Property(x => x.SectionCap).IsRequired();
        builder.Property(x => x.XpSectionTypeId).IsRequired();
        builder.Property(x => x.DiscretionXp).IsRequired();
        builder.Property(x => x.SpentXp).IsRequired();
        builder
            .Property(x => x.TotalCharacterCreationXp)
            .IsRequired();
        builder.Property(x => x.LevelXp).IsRequired();

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
