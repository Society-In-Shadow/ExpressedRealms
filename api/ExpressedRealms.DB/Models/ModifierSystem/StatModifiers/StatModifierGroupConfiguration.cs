using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;

public class StatModifierGroupConfiguration : IEntityTypeConfiguration<StatModifier>
{
    public void Configure(EntityTypeBuilder<StatModifier> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
    }
}
