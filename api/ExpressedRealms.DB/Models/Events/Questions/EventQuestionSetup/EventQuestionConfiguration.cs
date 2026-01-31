using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;

public class EventQuestionConfiguration : IEntityTypeConfiguration<EventQuestion>
{
    public void Configure(EntityTypeBuilder<EventQuestion> builder)
    {
        builder.ToTable("event_question");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();

        builder.Property(e => e.Question).HasColumnName("question").HasMaxLength(500).IsRequired();

        builder.Property(e => e.EventId).HasColumnName("event_id").IsRequired();

        builder.Property(x => x.QuestionTypeId).HasColumnName("question_type_id").IsRequired();

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.EventQuestions)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.QuestionType)
            .WithMany(x => x.EventQuestions)
            .HasForeignKey(x => x.QuestionTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
