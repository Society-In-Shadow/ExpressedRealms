using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Factions.FactionRankModels;

public class FactionRankConfiguration : IEntityTypeConfiguration<FactionRank>
{
    public void Configure(EntityTypeBuilder<FactionRank> builder)
    {
        var data = FactionRankEnum
            .List.Select(x => new FactionRank()
            {
                Id = x.Value,
                Name = x.ToString(),
            })
            .ToList();
        builder.HasData(data);
        
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
    }
}
