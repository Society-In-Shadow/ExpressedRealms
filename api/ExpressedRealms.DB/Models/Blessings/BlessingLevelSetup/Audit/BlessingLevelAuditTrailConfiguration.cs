using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.BlessingLevelSetup.Audit;

internal class BlessingLevelAuditTrailConfiguration
    : IEntityTypeConfiguration<BlessingLevelAuditTrail>
{
    public void Configure(EntityTypeBuilder<BlessingLevelAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.BlessingLevelAuditTrails);

        builder.Property(e => e.BlessingId).IsRequired();
        builder.Property(e => e.BlessingLevelId).IsRequired();

        builder
            .HasOne(x => x.Blessing)
            .WithMany(x => x.BlessingLevelAuditTrails)
            .HasForeignKey(x => x.BlessingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.BlessingLevel)
            .WithMany(x => x.BlessingLevelAuditTrails)
            .HasForeignKey(x => x.BlessingLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
