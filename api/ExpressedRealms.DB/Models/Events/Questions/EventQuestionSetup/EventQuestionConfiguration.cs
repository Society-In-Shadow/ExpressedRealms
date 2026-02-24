using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.Questions.EventQuestionSetup;

public class EventQuestionConfiguration : IEntityTypeConfiguration<EventQuestion>
{
    public void Configure(EntityTypeBuilder<EventQuestion> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();

        builder.Property(e => e.Question).HasMaxLength(500).IsRequired();

        builder.Property(e => e.EventId).IsRequired();

        builder.Property(x => x.QuestionTypeId).IsRequired();

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
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
