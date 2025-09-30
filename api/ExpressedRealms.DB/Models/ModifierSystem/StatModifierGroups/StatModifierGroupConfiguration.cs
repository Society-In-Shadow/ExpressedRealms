using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifierGroups;

public class StatModifierGroupConfiguration : IEntityTypeConfiguration<StatModifierGroup>
{
    public void Configure(EntityTypeBuilder<StatModifierGroup> builder)
    {
        builder.ToTable("stat_modifier_group");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
    }
}
