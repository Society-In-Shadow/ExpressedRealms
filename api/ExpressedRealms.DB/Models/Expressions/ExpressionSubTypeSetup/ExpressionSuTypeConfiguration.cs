using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionSubTypeSetup;

public class CmsTypeConfiguration : IEntityTypeConfiguration<ExpressionSubType>
{
    public void Configure(EntityTypeBuilder<ExpressionSubType> builder)
    {
        var data = ExpressionSubTypeEnum
            .List.Select(x => new ExpressionSubType()
            {
                Id = x.Value, 
                Name = x.ToString()
            })
            .ToList();
        builder.HasData(data);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();

        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
    }
}
