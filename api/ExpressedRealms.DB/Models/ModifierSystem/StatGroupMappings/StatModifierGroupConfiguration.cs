using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;

public class StatGroupMappingConfiguration : IEntityTypeConfiguration<StatGroupMapping>
{
    public void Configure(EntityTypeBuilder<StatGroupMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.StatGroupId).IsRequired();
        builder.Property(e => e.StatModifierId).IsRequired();
        builder.Property(e => e.Modifier).IsRequired();
        builder.Property(e => e.TargetExpressionId);

        builder
            .Property(e => e.CreationSpecificBonus)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .Property(e => e.ScaleWithLevel)
            .IsRequired()
            .HasDefaultValue(false);

        builder
            .HasOne(e => e.StatModifierGroup)
            .WithMany(e => e.StatGroupMappings)
            .HasForeignKey(e => e.StatGroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.StatModifier)
            .WithMany(e => e.StatGroupMappings)
            .HasForeignKey(e => e.StatModifierId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Expression)
            .WithMany(e => e.StatGroupMappings)
            .HasForeignKey(e => e.TargetExpressionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
