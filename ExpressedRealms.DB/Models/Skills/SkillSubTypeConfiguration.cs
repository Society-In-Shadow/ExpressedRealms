using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class SkillSubTypeConfiguration : IEntityTypeConfiguration<SkillSubType>
{
    public void Configure(EntityTypeBuilder<SkillSubType> builder)
    {
        builder.ToTable(nameof(SkillSubType));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasMaxLength(10).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(125).IsRequired();
    }
}
