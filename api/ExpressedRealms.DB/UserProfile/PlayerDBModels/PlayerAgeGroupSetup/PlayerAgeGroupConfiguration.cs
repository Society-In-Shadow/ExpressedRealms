using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;

public class PlayerAgeGroupConfiguration : IEntityTypeConfiguration<PlayerAgeGroup>
{
    public void Configure(EntityTypeBuilder<PlayerAgeGroup> builder)
    {
        var data = PlayerAgeGroupEnum
            .List.Select(x => new PlayerAgeGroup { Id = x.Value, Name = x.ToString() })
            .ToList();
        builder.HasData(data);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
    }
}
