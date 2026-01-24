using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Contacts;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("contact");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.CharacterId).HasColumnName("character_id").IsRequired();
        builder.Property(e => e.KnowledgeId).HasColumnName("knowledge_id").IsRequired();
        builder.Property(e => e.KnowledgeLevelId).HasColumnName("knowledge_level_id").IsRequired();
        builder.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(5000);
        builder.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(300);
        builder.Property(e => e.Frequency).HasColumnName("frequency").IsRequired();
        builder.Property(e => e.SpentXp).HasColumnName("spent_xp").IsRequired();
        builder.Property(e => e.IsApproved).HasColumnName("is_approved");

        builder
            .HasOne(e => e.Knowledge)
            .WithMany(e => e.Contacts)
            .HasForeignKey(e => e.KnowledgeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Character)
            .WithMany(e => e.Contacts)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.KnowledgeLevel)
            .WithMany(e => e.Contacts)
            .HasForeignKey(e => e.KnowledgeLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
