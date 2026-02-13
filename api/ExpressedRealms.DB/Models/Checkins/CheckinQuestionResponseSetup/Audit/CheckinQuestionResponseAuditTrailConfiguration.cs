using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup.Audit;

internal class CheckinQuestionResponseAuditTrailConfiguration
    : IEntityTypeConfiguration<CheckinQuestionResponseAuditTrail>
{
    public void Configure(EntityTypeBuilder<CheckinQuestionResponseAuditTrail> builder)
    {
        builder.ToTable("checkin_event_question_response_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.CheckinQuestionResponseAuditTrails);

        builder.Property(e => e.CheckinId).HasColumnName("checkin_id").IsRequired();
        builder.Property(e => e.EventQuestionId).HasColumnName("event_question_id").IsRequired();

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
