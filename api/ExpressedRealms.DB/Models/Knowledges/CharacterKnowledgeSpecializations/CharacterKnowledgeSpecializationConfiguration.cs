using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;

public class CharacterKnowledgeSpecializationConfiguration : IEntityTypeConfiguration<CharacterKnowledgeSpecialization>
{
    public void Configure(EntityTypeBuilder<CharacterKnowledgeSpecialization> builder)
    {
        builder.ToTable("character_knowledge_specialization");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.KnowledgeMappingId).HasColumnName("knowledge_mapping_id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").HasMaxLength(5000).IsRequired();
        builder.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(10000);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
        
        builder
            .HasOne(e => e.CharacterKnowledgeMapping)
            .WithMany(e => e.CharacterKnowledgeSpecializations)
            .HasForeignKey(e => e.KnowledgeMappingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
