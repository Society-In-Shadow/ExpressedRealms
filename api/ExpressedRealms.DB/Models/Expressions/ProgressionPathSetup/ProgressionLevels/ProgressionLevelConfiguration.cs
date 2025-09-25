using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels;

public class ProgressionLevelConfiguration : IEntityTypeConfiguration<ProgressionLevel>
{
    public void Configure(EntityTypeBuilder<ProgressionLevel> builder)
    {
        builder.ToTable("progression_level");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.ProgressionPathId).HasColumnName("progression_path_id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(5000).IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
