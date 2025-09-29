using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Characters;

public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Background);
        builder.Property(x => x.PlayerId).IsRequired();
        builder.Property(x => x.ExpressionId).IsRequired();

        builder.Property(x => x.AgilityId).IsRequired().HasDefaultValue(1);
        builder.Property(x => x.ConstitutionId).IsRequired().HasDefaultValue(1);
        builder.Property(x => x.DexterityId).IsRequired().HasDefaultValue(1);
        builder.Property(x => x.StrengthId).IsRequired().HasDefaultValue(1);
        builder.Property(x => x.IntelligenceId).IsRequired().HasDefaultValue(1);
        builder.Property(x => x.WillpowerId).IsRequired().HasDefaultValue(1);

        builder.Property(x => x.FactionId);

        builder.Property(x => x.StatExperiencePoints).IsRequired().HasDefaultValue(72);

        builder.Property(x => x.PrimaryProgressionId).HasColumnName("primary_progression_id");
        builder.Property(x => x.SecondaryProgressionId).HasColumnName("secondary_progression_id");

        builder
            .HasOne(x => x.PrimaryProgressionPath)
            .WithMany(x => x.PrimaryProgressions)
            .HasForeignKey(x => x.PrimaryProgressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder
            .HasOne(x => x.SecondaryProgressionPath)
            .WithMany(x => x.SecondaryProgressions)
            .HasForeignKey(x => x.SecondaryProgressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        builder
            .Property(x => x.IsInCharacterCreation)
            .HasColumnName("is_in_character_creation")
            .IsRequired()
            .HasDefaultValue(true);
        builder
            .Property(x => x.IsPrimaryCharacter)
            .HasColumnName("is_primary_character")
            .IsRequired()
            .HasDefaultValue(false);
        builder
            .Property(x => x.AssignedXp)
            .HasColumnName("assigned_xp")
            .IsRequired()
            .HasDefaultValue(0);

        builder
            .Property(x => x.PlayerNumber)
            .HasColumnName("player_number")
            .IsRequired()
            .HasDefaultValue(0);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasOne(x => x.Player)
            .WithMany(x => x.Characters)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.Characters)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.AgilityStatLevel)
            .WithMany(x => x.CharacterAgility)
            .HasForeignKey(x => x.AgilityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ConstitutionStatLevel)
            .WithMany(x => x.CharacterConstitution)
            .HasForeignKey(x => x.ConstitutionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.DexterityStatLevel)
            .WithMany(x => x.CharacterDexterity)
            .HasForeignKey(x => x.DexterityId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.StrengthStatLevel)
            .WithMany(x => x.CharacterStrength)
            .HasForeignKey(x => x.StrengthId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.IntelligenceStatLevel)
            .WithMany(x => x.CharacterIntelligence)
            .HasForeignKey(x => x.IntelligenceId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.WillpowerStatLevel)
            .WithMany(x => x.CharacterWillpower)
            .HasForeignKey(x => x.WillpowerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.FactionInfo)
            .WithMany(x => x.CharactersList)
            .HasForeignKey(x => x.FactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
