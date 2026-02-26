using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;

internal class ProgressionLevelAuditTrailConfiguration
    : IEntityTypeConfiguration<ProgressionLevelAuditTrail>
{
    public void Configure(EntityTypeBuilder<ProgressionLevelAuditTrail> builder)
    {
        builder.Property(e => e.ProgressionLevelId).IsRequired();
        builder.Property(e => e.ProgressionPathId).IsRequired();

        builder
            .HasOne(x => x.ProgressionPath)
            .WithMany(x => x.ProgressionLevelAuditTrails)
            .HasForeignKey(x => x.ProgressionPathId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.ProgressionLevel)
            .WithMany(x => x.ProgressionLevelAuditTrails)
            .HasForeignKey(x => x.ProgressionLevelId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.ProgressionLevelAuditTrails);
    }
}
