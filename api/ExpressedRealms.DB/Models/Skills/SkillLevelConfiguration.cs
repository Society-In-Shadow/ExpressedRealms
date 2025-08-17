using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class SkillLevelConfiguration : IEntityTypeConfiguration<SkillLevel>
{
    public void Configure(EntityTypeBuilder<SkillLevel> builder)
    {
        builder.ToTable("skill_level");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(10).IsRequired();
        builder.Property(e => e.XP).HasColumnName("xp").IsRequired();
        builder.Property(e => e.TotalXp).HasColumnName("total_xp").IsRequired().HasDefaultValue(0);
        builder.Property(e => e.Level).HasColumnName("level").IsRequired();
    }
}
