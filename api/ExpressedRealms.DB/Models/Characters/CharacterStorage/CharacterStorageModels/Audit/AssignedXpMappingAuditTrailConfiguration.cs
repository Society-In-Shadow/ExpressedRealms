using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.CharacterStorage.CharacterStorageModels.Audit;

internal class AssignedXpMappingAuditTrailConfiguration
    : IEntityTypeConfiguration<CharacterStorageInfoAuditTrail>
{
    public void Configure(EntityTypeBuilder<CharacterStorageInfoAuditTrail> builder)
    {
        builder.Property(e => e.CharacterStorageInfoId).IsRequired();

        builder
            .HasOne(x => x.CharacterStorageInfo)
            .WithMany(x => x.CharacterStorageInfoAuditTrails)
            .HasForeignKey(x => x.CharacterStorageInfoId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.CharacterStorageInfoAuditTrails);
    }
}
