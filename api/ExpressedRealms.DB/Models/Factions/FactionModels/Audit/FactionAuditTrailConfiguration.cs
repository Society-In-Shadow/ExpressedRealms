using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Factions.FactionModels.Audit;

internal class FactionAuditTrailConfiguration : IEntityTypeConfiguration<FactionAuditTrail>
{
    public void Configure(EntityTypeBuilder<FactionAuditTrail> builder)
    {
        builder.Property(e => e.FactionId).IsRequired();

        builder
            .HasOne(x => x.Faction)
            .WithMany(x => x.FactionAuditTrails)
            .HasForeignKey(x => x.FactionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.FactionAuditTrails);
    }
}
