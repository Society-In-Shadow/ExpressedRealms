using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Factions.CharacterFactionMappingModels;

public class FactionLevelConfiguration : IEntityTypeConfiguration<CharacterFactionMapping>
{
    public void Configure(EntityTypeBuilder<CharacterFactionMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).IsRequired();
        builder.Property(e => e.CharacterId).IsRequired();
        builder.Property(e => e.FactionLevelId).IsRequired();

        builder.Property(e => e.ApprovedByUserId).IsRequired(false).HasMaxLength(450);
        builder.Property(e => e.RequestReason).HasMaxLength(20_000);
        builder.Property(e => e.CharacterNotes).HasMaxLength(20_000);
        builder.Property(e => e.ApprovalReason).HasMaxLength(20_000);
        builder.Property(e => e.ApprovalDate).IsRequired();
        
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasOne(x => x.ApprovedByUser)
            .WithMany(x => x.CharacterFactionMappings)
            .HasForeignKey(x => x.ApprovedByUserId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.FactionLevel)
            .WithMany(x => x.CharacterFactionMappings)
            .HasForeignKey(x => x.FactionLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.Character)
            .WithMany(x => x.CharacterFactionMappings)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
