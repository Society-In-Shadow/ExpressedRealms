using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;

public class QuestionTypeConfiguration : IEntityTypeConfiguration<QuestionType>
{
    public void Configure(EntityTypeBuilder<QuestionType> builder)
    {
        var data = QuestionTypeEnum
            .List.Select(x => new QuestionType
            {
                Id = x.Value,
                Name = x.ToString(),
                IsDefault = x.IsApproved,
                IsCustomizable = x.IsCustomizable,
            })
            .ToList();
        builder.HasData(data);

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();

        builder.Property(e => e.IsDefault).IsRequired();

        builder.Property(e => e.IsCustomizable).IsRequired();
    }
}
