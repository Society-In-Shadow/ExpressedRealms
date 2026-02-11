using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Checkins.CheckinQuestionResponseSetup;

public class CheckinQuestionResponseConfiguration
    : IEntityTypeConfiguration<CheckinQuestionResponse>
{
    public void Configure(EntityTypeBuilder<CheckinQuestionResponse> builder)
    {
        builder.ToTable("checkin_question_response");

        builder.HasKey(e => new { e.CheckinId, e.EventQuestionId });
        builder.Property(e => e.CheckinId).HasColumnName("checkin_id").IsRequired();
        builder.Property(e => e.EventQuestionId).HasColumnName("event_question_id").IsRequired();
        builder.Property(e => e.Response).HasColumnName("response").IsRequired();

        builder
            .HasOne(x => x.Checkin)
            .WithMany(x => x.CheckinQuestionResponses)
            .HasForeignKey(x => x.CheckinId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.EventQuestion)
            .WithMany(x => x.CheckinQuestionResponses)
            .HasForeignKey(x => x.EventQuestionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
