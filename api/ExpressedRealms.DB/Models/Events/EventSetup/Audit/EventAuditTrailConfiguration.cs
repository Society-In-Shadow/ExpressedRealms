using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.EventSetup.Audit;

internal class EventAuditTrailConfiguration : IEntityTypeConfiguration<EventAuditTrail>
{
    public void Configure(EntityTypeBuilder<EventAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.EventAuditTrails);

        builder.Property(e => e.EventId).IsRequired();

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.EventAuditTrails)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
