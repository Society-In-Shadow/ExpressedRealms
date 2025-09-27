using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;

public class ProgressionPathConfiguration : IEntityTypeConfiguration<ProgressionPath>
{
    public void Configure(EntityTypeBuilder<ProgressionPath> builder)
    {
        builder.ToTable("progression_path");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.ExpressionId).HasColumnName("expression_id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder
            .Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(5000)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
