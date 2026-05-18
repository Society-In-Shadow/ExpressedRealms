using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Statistics.CharacterStatMappings;

public class CharacterStatMappingConfiguration : IEntityTypeConfiguration<CharacterStatMapping>
{
    public void Configure(EntityTypeBuilder<CharacterStatMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CharacterId).IsRequired();
        builder.Property(e => e.StatTypeId).IsRequired();
        builder.Property(e => e.StatLevelId).IsRequired();
        
        builder
            .HasOne(e => e.StatType)
            .WithMany(e => e.CharacterStatMappings)
            .HasForeignKey(e => e.StatTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(e => e.StatLevel)
            .WithMany(e => e.CharacterStatMappings)
            .HasForeignKey(e => e.StatLevelId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .HasOne(e => e.Character)
            .WithMany(e => e.CharacterStatMappings)
            .HasForeignKey(e => e.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
