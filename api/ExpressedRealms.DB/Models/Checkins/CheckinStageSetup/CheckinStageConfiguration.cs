using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;

public class CheckinStageConfiguration : IEntityTypeConfiguration<CheckinStage>
{
    public void Configure(EntityTypeBuilder<CheckinStage> builder)
    {
        var data = CheckinStageEnum
            .List.Select(x => new CheckinStage
            {
                Id = x.Value,
                Name = x.ToString(),
                Description = x.Description,
            })
            .ToList();
        builder.HasData(data);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).IsRequired().HasMaxLength(250);
        builder.Property(e => e.Description).IsRequired().HasMaxLength(5000);
    }
}
