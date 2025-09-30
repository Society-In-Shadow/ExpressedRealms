using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;

public class StatModifierGroupConfiguration : IEntityTypeConfiguration<StatModifier>
{
    public void Configure(EntityTypeBuilder<StatModifier> builder)
    {
        builder.ToTable("stat_modifier");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
    }
}
