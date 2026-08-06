using ExpressedRealms.DB.Models.Expressions.ExpressionPublishStatusSetup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions;

public class ExpressionPublishStatusConfiguration
    : IEntityTypeConfiguration<ExpressionPublishStatus>
{
    public void Configure(EntityTypeBuilder<ExpressionPublishStatus> builder)
    {
        var data = ExpressionPublishStatusEnum
            .List.Select(x => new ExpressionPublishStatus()
            {
                Id = x.Value,
                Name = x.ToString(),
                Description = x.Description,
            })
            .ToList();
        builder.HasData(data);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(250).IsRequired();
    }
}
