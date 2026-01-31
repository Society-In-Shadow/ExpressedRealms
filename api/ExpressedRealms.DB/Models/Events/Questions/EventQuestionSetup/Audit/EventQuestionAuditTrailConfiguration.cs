using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup.Audit;

internal class EventQuestionAuditTrailConfiguration : IEntityTypeConfiguration<EventQuestionAuditTrail>
{
    public void Configure(EntityTypeBuilder<EventQuestionAuditTrail> builder)
    {
        builder.ToTable("event_question_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.EventQuestionAuditTrails);

        builder.Property(e => e.EventQuestionId).HasColumnName("event_question_id").IsRequired();

        builder
            .HasOne(x => x.EventQuestion)
            .WithMany(x => x.EventQuestionAuditTrails)
            .HasForeignKey(x => x.EventQuestionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
