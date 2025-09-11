using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;

public class CharacterBlessingMappingConfiguration : IEntityTypeConfiguration<CharacterBlessingMapping>
{
    public void Configure(EntityTypeBuilder<CharacterBlessingMapping> builder)
    {
        builder.ToTable("character_blessing_mapping");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired();
        builder.Property(e => e.CharacterId).HasColumnName("character_id").IsRequired();
        builder.Property(e => e.BlessingId).HasColumnName("blessing_id").IsRequired();
        builder.Property(e => e.BlessingLevelId).HasColumnName("blessing_level_id").IsRequired();
        builder.Property(e => e.Notes).HasColumnName("notes").HasMaxLength(5000);

        builder
            .HasOne(e => e.Blessing)
            .WithMany(e => e.CharacterBlessingMappings)
            .HasForeignKey(e => e.BlessingId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Character)
            .WithMany(e => e.CharacterBlessingMappings)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.BlessingLevel)
            .WithMany(e => e.CharacterBlessingMappings)
            .HasForeignKey(e => e.BlessingLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted).HasColumnName("is_deleted");
        builder.Property(e => e.DeletedAt).HasColumnName("deleted_at");
    }
}
