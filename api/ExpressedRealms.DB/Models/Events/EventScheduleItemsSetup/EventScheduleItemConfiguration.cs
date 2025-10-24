using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup;

public class EventScheduleItemConfiguration : IEntityTypeConfiguration<EventScheduleItem>
{
    public void Configure(EntityTypeBuilder<EventScheduleItem> builder)
    {
        builder.ToTable("event_schedule_item");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.EventId).HasColumnName("event_id").IsRequired();
        builder
            .Property(e => e.Description)
            .HasColumnName("description")
            .HasMaxLength(250)
            .IsRequired();
        builder.Property(e => e.Date).HasColumnName("date").IsRequired();
        builder.Property(e => e.StartTime).HasColumnName("start_time").IsRequired();
        builder.Property(e => e.EndTime).HasColumnName("end_time").IsRequired();
        
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
