using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Statistics;

public class StatTypeConfiguration : IEntityTypeConfiguration<StatType>
{
    public void Configure(EntityTypeBuilder<StatType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(15);
        builder.Property(x => x.ShortName).IsRequired().HasMaxLength(3);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(200);
    }
}
