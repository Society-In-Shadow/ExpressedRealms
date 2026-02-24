using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;

public class EventScheduleItemConfiguration : IEntityTypeConfiguration<EventScheduleItem>
{
    public void Configure(EntityTypeBuilder<EventScheduleItem> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.EventId).IsRequired();
        builder
            .Property(e => e.Description)
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(e => e.Date).IsRequired();
        builder.Property(e => e.StartTime).IsRequired();
        builder.Property(e => e.EndTime).IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
