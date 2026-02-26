using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels;

public class AssignedXpMappingConfiguration : IEntityTypeConfiguration<AssignedXpMapping>
{
    public void Configure(EntityTypeBuilder<AssignedXpMapping> builder)
    {
        builder.HasQueryFilter(x => !x.IsDeleted);
        builder.Property(e => e.IsDeleted);
        builder.Property(e => e.DeletedAt);

        builder
            .HasOne(x => x.Character)
            .WithMany(x => x.AssignedXpMappings)
            .HasForeignKey(x => x.CharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Event)
            .WithMany(x => x.AssignedXpMappings)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Player)
            .WithMany(x => x.AssignedXpMappings)
            .HasForeignKey(x => x.PlayerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.AssignedXpType)
            .WithMany(x => x.AssignedXpMappings)
            .HasForeignKey(x => x.AssignedXpTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.AssignedByUser)
            .WithMany(x => x.AssignedXpMappings)
            .HasForeignKey(x => x.AssignedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
