using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Blessings.BlessingSetup.Audit;

internal class BlessingAuditTrailConfiguration : IEntityTypeConfiguration<BlessingAuditTrail>
{
    public void Configure(EntityTypeBuilder<BlessingAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.BlessingAuditTrails);

        builder.Property(e => e.BlessingId).IsRequired();

        builder
            .HasOne(x => x.Blessing)
            .WithMany(x => x.BlessingAuditTrails)
            .HasForeignKey(x => x.BlessingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
