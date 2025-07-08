using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Knowledges.KnowledgeModels;

public class KnowledgeConfiguration : IEntityTypeConfiguration<Knowledge>
{
    public void Configure(EntityTypeBuilder<Knowledge> builder)
    {
        builder.ToTable("knowledge");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.KnowledgeTypeId).HasColumnName("knowledge_type_id").IsRequired();
        builder.Property(e => e.Name).HasColumnName("name").HasMaxLength(250).IsRequired();
        builder.Property(e => e.Description).HasColumnName("description").IsRequired();

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
