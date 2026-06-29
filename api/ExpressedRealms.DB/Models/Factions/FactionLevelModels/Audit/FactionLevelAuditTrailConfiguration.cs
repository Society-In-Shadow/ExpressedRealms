using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Factions.FactionLevelModels.Audit;

internal class FactionLevelAuditTrailConfiguration : IEntityTypeConfiguration<FactionLevelAuditTrail>
{
    public void Configure(EntityTypeBuilder<FactionLevelAuditTrail> builder)
    {
        builder.Property(e => e.FactionLevelId).IsRequired();

        builder
            .HasOne(x => x.FactionLevel)
            .WithMany(x => x.FactionLevelAuditTrails)
            .HasForeignKey(x => x.FactionLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.FactionLevelAuditTrails);
    }
}
