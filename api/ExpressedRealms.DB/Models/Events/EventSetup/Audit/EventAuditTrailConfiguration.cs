using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.EventSetup.Audit;

internal class EventAuditTrailConfiguration : IEntityTypeConfiguration<EventAuditTrail>
{
    public void Configure(EntityTypeBuilder<EventAuditTrail> builder)
    {
        builder.ToTable("event_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.EventAuditTrails);

        builder.Property(e => e.EventId).HasColumnName("event_id").IsRequired();

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.EventAuditTrails)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
