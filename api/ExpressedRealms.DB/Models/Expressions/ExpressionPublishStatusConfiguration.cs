using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionPublishStatusConfiguration
    : IEntityTypeConfiguration<ExpressionPublishStatus>
{
    public void Configure(EntityTypeBuilder<ExpressionPublishStatus> builder)
    {
        builder.ToTable(nameof(ExpressionPublishStatus));

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(250).IsRequired();
    }
}
