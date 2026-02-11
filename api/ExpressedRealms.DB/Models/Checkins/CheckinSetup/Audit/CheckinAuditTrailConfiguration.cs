using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinSetup.Audit;

internal class CheckinAuditTrailConfiguration : IEntityTypeConfiguration<CheckinAuditTrail>
{
    public void Configure(EntityTypeBuilder<CheckinAuditTrail> builder)
    {
        builder.ToTable("checkin_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.CheckinAuditTrails);

        builder.Property(e => e.CheckinId).HasColumnName("checkin_id").IsRequired();

        builder
            .HasOne(x => x.Checkin)
            .WithMany(x => x.CheckinAuditTrails)
            .HasForeignKey(x => x.CheckinId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
