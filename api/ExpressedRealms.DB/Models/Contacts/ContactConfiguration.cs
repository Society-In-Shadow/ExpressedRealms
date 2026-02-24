using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Contacts;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CharacterId).IsRequired();
        builder.Property(e => e.KnowledgeId).IsRequired();
        builder.Property(e => e.KnowledgeLevelId).IsRequired();
        builder.Property(e => e.Notes).HasMaxLength(5000);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(300);
        builder.Property(e => e.Frequency).IsRequired();
        builder.Property(e => e.SpentXp).IsRequired();
        builder.Property(e => e.IsApproved);

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
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
