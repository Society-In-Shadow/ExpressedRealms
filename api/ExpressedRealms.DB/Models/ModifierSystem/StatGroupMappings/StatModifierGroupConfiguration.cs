using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings;

public class StatGroupMappingConfiguration : IEntityTypeConfiguration<StatGroupMapping>
{
    public void Configure(EntityTypeBuilder<StatGroupMapping> builder)
    {
        builder.ToTable("stat_group_mapping");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.StatGroupId).HasColumnName("stat_group_id").IsRequired();
        builder.Property(e => e.StatModifierId).HasColumnName("stat_modifier_id").IsRequired();
        builder.Property(e => e.Modifier).HasColumnName("modifier").IsRequired();

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
    }
}
