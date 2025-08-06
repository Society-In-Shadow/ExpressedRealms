using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeMappings;

public class CharacterKnowledgeMappingConfiguration
    : IEntityTypeConfiguration<CharacterKnowledgeMapping>
{
    public void Configure(EntityTypeBuilder<CharacterKnowledgeMapping> builder)
    {
        builder.ToTable("character_knowledge_mapping");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.CharacterId).HasColumnName("character_id").IsRequired();
        builder.Property(e => e.KnowledgeId).HasColumnName("knowledge_id").IsRequired();
        builder.Property(e => e.KnowledgeLevelId).HasColumnName("knowledge_level_id").IsRequired();
        builder.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(5000);

        builder
            .HasOne(e => e.Knowledge)
            .WithMany(e => e.CharacterKnowledgeMappings)
            .HasForeignKey(e => e.KnowledgeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Character)
            .WithMany(e => e.CharacterKnowledgeMappings)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.KnowledgeLevel)
            .WithMany(e => e.CharacterKnowledgeMappings)
            .HasForeignKey(e => e.KnowledgeLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
