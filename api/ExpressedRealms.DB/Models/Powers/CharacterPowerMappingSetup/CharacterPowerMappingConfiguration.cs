using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Powers.CharacterPowerMappingSetup;

public class CharacterPowerMappingConfiguration : IEntityTypeConfiguration<CharacterPowerMapping>
{
    public void Configure(EntityTypeBuilder<CharacterPowerMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CharacterId).IsRequired();
        builder.Property(e => e.PowerId).IsRequired();
        builder.Property(e => e.PowerLevelId).IsRequired();
        builder.Property(e => e.Notes).HasMaxLength(5000);

        builder
            .HasOne(e => e.Power)
            .WithMany(e => e.CharacterPowerMappings)
            .HasForeignKey(e => e.PowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.Character)
            .WithMany(e => e.CharacterPowerMappings)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(e => e.PowerLevel)
            .WithMany(e => e.CharacterPowerMappings)
            .HasForeignKey(e => e.PowerLevelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);
    }
}
