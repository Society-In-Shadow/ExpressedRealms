using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionSectionsConfiguration : IEntityTypeConfiguration<ExpressionSection>
{
    public void Configure(EntityTypeBuilder<ExpressionSection> builder)
    {
        builder.ToTable("ExpressionSections");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(e => e.ExpressionId).IsRequired();

        builder.Property(e => e.SectionTypeId).IsRequired();

        builder.Property(e => e.ParentId);

        builder.Property(e => e.Name).HasMaxLength(150).IsRequired();

        builder.Property(e => e.Content).IsRequired();

        builder
            .HasOne(e => e.Parent)
            .WithMany(e => e.Children)
            .HasForeignKey(e => e.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.ExpressionSections)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.SectionType)
            .WithMany(x => x.ExpressionSections)
            .HasForeignKey(x => x.SectionTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
