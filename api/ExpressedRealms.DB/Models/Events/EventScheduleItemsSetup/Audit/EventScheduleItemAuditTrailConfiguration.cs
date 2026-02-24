using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.EventScheduleItemsSetup.Audit;

internal class EventScheduleItemAuditTrailConfiguration
    : IEntityTypeConfiguration<EventScheduleItemAuditTrail>
{
    public void Configure(EntityTypeBuilder<EventScheduleItemAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.EventScheduleItemAuditTrails);

        builder
            .Property(e => e.EventScheduleItemId)
            .IsRequired();

        builder
            .HasOne(x => x.EventScheduleItem)
            .WithMany(x => x.EventScheduleItemAuditTrails)
            .HasForeignKey(x => x.EventScheduleItemId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.EventScheduleAuditTrails)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
