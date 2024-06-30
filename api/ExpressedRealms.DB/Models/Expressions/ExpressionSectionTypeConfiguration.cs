using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionSectionTypeConfiguration : IEntityTypeConfiguration<ExpressionSectionType>
{
    public void Configure(EntityTypeBuilder<ExpressionSectionType> builder)
    {
        builder.ToTable("ExpressionSectionTypes");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();

        builder.Property(e => e.Description);
    }
}
