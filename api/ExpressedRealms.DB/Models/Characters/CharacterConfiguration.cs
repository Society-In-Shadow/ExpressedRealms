using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters;

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

        builder.Property(x => x.FactionId);

        builder.Property(x => x.StatExperiencePoints).IsRequired().HasDefaultValue(72);

        builder.Property(x => x.WealthLevel);
        builder.Property(x => x.PrimaVoid);

        builder.Property(x => x.PrimaryProgressionId);
        builder.Property(x => x.SecondaryProgressionId);

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

        builder.Property(x => x.IsInCharacterCreation).IsRequired().HasDefaultValue(true);
        builder.Property(x => x.IsPrimaryCharacter).IsRequired().HasDefaultValue(false);

        builder.Property(x => x.IsRetired).IsRequired().HasDefaultValue(false);

        builder.Property(x => x.RetiredDate);

        builder.Property(x => x.PlayerNumber).IsRequired().HasDefaultValue(0);

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
            .HasOne(x => x.FactionInfo)
            .WithMany(x => x.CharactersList)
            .HasForeignKey(x => x.FactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
