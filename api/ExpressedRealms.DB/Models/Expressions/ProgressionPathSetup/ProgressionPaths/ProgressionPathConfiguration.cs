using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths;

public class ProgressionPathConfiguration : IEntityTypeConfiguration<ProgressionPath>
{
    public void Configure(EntityTypeBuilder<ProgressionPath> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.ExpressionId).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder
            .Property(e => e.Description)
            .HasMaxLength(5000)
            .IsRequired();

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.ProgressionPaths)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
