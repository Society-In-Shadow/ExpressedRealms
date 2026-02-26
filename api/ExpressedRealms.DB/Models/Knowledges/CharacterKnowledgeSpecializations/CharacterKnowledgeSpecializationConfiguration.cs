using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.CharacterKnowledgeSpecializations;

public class CharacterKnowledgeSpecializationConfiguration
    : IEntityTypeConfiguration<CharacterKnowledgeSpecialization>
{
    public void Configure(EntityTypeBuilder<CharacterKnowledgeSpecialization> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.KnowledgeMappingId).IsRequired();
        builder.Property(e => e.Name).HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasMaxLength(5000).IsRequired();
        builder.Property(e => e.Notes).HasMaxLength(10000);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);

        builder
            .HasOne(e => e.CharacterKnowledgeMapping)
            .WithMany(e => e.CharacterKnowledgeSpecializations)
            .HasForeignKey(e => e.KnowledgeMappingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
