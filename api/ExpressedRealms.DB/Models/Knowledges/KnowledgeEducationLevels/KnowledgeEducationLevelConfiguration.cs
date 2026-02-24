using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;

public class KnowledgeEducationLevelConfiguration
    : IEntityTypeConfiguration<KnowledgeEducationLevel>
{
    public void Configure(EntityTypeBuilder<KnowledgeEducationLevel> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(50).IsRequired();
        builder.Property(e => e.Level).IsRequired();
        builder
            .Property(e => e.SpecializationCount)
            .IsRequired();
        builder.Property(e => e.StoneModifier).IsRequired();
        builder.Property(e => e.GeneralXpCost).IsRequired();
        builder
            .Property(e => e.TotalGeneralXpCost)
            .IsRequired();
        builder.Property(e => e.UnknownXpCost).IsRequired();
        builder
            .Property(e => e.TotalUnknownXpCost)
            .IsRequired();
    }
}
