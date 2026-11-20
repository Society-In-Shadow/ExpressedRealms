using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Creatures.PublishType;

public class CreaturePublishTypeConfiguration : IEntityTypeConfiguration<CreaturePublishType>
{
    public void Configure(EntityTypeBuilder<CreaturePublishType> builder)
    {
        var data = CreaturePublishTypeEnum
            .List.Select(x => new CreaturePublishType()
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
