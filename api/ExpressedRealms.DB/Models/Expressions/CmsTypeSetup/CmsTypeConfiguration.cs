using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.CmsTypeSetup;

public class CmsTypeConfiguration : IEntityTypeConfiguration<CmsType>
{
    public void Configure(EntityTypeBuilder<CmsType> builder)
    {
        var data = CmsTypeEnum
            .List.Select(x => new CmsType()
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
