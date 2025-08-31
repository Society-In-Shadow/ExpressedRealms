using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class ModifierTypeConfiguration : IEntityTypeConfiguration<ModifierType>
{
    public void Configure(EntityTypeBuilder<ModifierType> builder)
    {
        builder.ToTable("modifier_type");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(75).IsRequired();
    }
}
