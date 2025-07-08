using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges;

public class KnowledgeTypeConfiguration : IEntityTypeConfiguration<KnowledgeType>
{
    public void Configure(EntityTypeBuilder<KnowledgeType> builder)
    {
        builder.ToTable("knowledge_type");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").IsRequired();

        builder
            .HasMany(e => e.Knowledges)
            .WithOne(e => e.KnowledgeType)
            .HasForeignKey(e => e.KnowledgeTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
