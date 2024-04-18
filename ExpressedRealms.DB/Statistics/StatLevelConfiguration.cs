using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Statistics;

public class StatLevelConfiguration : IEntityTypeConfiguration<StatLevel>
{
    public void Configure(EntityTypeBuilder<StatLevel> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Bonus).IsRequired();
        builder.Property(x => x.XPCost).IsRequired();
    }
}