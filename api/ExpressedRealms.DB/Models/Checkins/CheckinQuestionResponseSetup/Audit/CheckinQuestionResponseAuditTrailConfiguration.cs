using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup.Audit;

internal class CheckinQuestionResponseAuditTrailConfiguration
    : IEntityTypeConfiguration<CheckinQuestionResponseAuditTrail>
{
    public void Configure(EntityTypeBuilder<CheckinQuestionResponseAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.CheckinQuestionResponseAuditTrails);

        builder.Property(e => e.CheckinId).IsRequired();
        builder.Property(e => e.EventQuestionId).IsRequired();

        builder
            .HasOne(x => x.Checkin)
            .WithMany(x => x.CheckinQuestionResponseAuditTrails)
            .HasForeignKey(x => x.CheckinId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.CheckinQuestionResponse)
            .WithMany(x => x.CheckinQuestionResponseAuditTrails)
            .HasForeignKey(x => new { x.CheckinId, x.EventQuestionId })
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.EventQuestion)
            .WithMany(x => x.CheckinQuestionResponseAuditTrails)
            .HasForeignKey(x => x.EventQuestionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
