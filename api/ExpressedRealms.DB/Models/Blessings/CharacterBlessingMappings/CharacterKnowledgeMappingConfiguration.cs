using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.CharacterBlessingMappings;

public class CharacterBlessingMappingConfiguration
    : IEntityTypeConfiguration<CharacterBlessingMapping>
{
    public void Configure(EntityTypeBuilder<CharacterBlessingMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CharacterId).IsRequired();
        builder.Property(e => e.BlessingId).IsRequired();
        builder.Property(e => e.BlessingLevelId).IsRequired();
        builder.Property(e => e.Notes).HasMaxLength(5000);

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
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
