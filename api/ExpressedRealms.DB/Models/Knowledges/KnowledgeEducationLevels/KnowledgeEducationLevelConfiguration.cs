using ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.KnowledgeEducationLevels;

public class KnowledgeEducationLevelConfiguration
    : IEntityTypeConfiguration<KnowledgeEducationLevel>
{
    public void Configure(EntityTypeBuilder<KnowledgeEducationLevel> builder)
    {
        builder.ToTable("knowledge_education_level");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(50).IsRequired();
        builder.Property(e => e.Level).HasColumnName("level").IsRequired();
        builder
            .Property(e => e.SpecializationCount)
            .HasColumnName("specialization_count")
            .IsRequired();
        builder.Property(e => e.StoneModifier).HasColumnName("stone_modifier").IsRequired();
        builder.Property(e => e.GeneralXpCost).HasColumnName("general_xp_cost").IsRequired();
        builder.Property(e => e.UnknownXpCost).HasColumnName("unknown_xp_cost").IsRequired();
    }
}
