using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;

public class StatModifierConfiguration : IEntityTypeConfiguration<StatModifier>
{
    public void Configure(EntityTypeBuilder<StatModifier> builder)
    {
        var data = StatModifierEnum
            .List.Select(x => new StatModifier { Id = x.Value, Name = x.ToString() })
            .ToList();
        builder.HasData(data);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
    }
}
