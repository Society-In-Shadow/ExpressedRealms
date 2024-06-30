using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Skills;

public class SkillTypeConfiguration : IEntityTypeConfiguration<SkillType>
{
    public void Configure(EntityTypeBuilder<SkillType> builder)
    {
        builder.ToTable(nameof(SkillType));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasMaxLength(25).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(500).IsRequired().IsRequired();

        builder.Property(e => e.SkillSubTypeId).IsRequired();
        builder
            .HasOne(e => e.SkillSubType)
            .WithMany(e => e.SkillTypes)
            .HasForeignKey(e => e.SkillSubTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
