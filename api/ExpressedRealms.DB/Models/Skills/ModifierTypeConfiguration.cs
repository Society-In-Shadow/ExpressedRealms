using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class ModifierTypeConfiguration : IEntityTypeConfiguration<ModifierType>
{
    public void Configure(EntityTypeBuilder<ModifierType> builder)
    {
        builder.ToTable(nameof(ModifierType));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500).IsRequired();
    }
}
