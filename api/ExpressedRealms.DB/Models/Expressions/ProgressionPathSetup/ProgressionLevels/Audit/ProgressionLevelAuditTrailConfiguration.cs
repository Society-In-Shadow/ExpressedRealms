using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionLevels.Audit;

internal class ProgressionLevelAuditTrailConfiguration
    : IEntityTypeConfiguration<ProgressionLevelAuditTrail>
{
    public void Configure(EntityTypeBuilder<ProgressionLevelAuditTrail> builder)
    {
        builder.ToTable("progression_level_audit_trail");

        builder
            .Property(e => e.ProgressionLevelId)
            .HasColumnName("progression_level_id")
            .IsRequired();
        builder
            .Property(e => e.ProgressionPathId)
            .HasColumnName("progression_path_id")
            .IsRequired();

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
