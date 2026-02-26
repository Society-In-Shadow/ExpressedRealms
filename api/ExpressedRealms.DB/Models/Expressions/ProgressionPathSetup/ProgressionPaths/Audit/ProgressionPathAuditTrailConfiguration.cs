using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Expressions.ProgressionPathSetup.ProgressionPaths.Audit;

internal class ProgressionPathAuditTrailConfiguration
    : IEntityTypeConfiguration<ProgressionPathAuditTrail>
{
    public void Configure(EntityTypeBuilder<ProgressionPathAuditTrail> builder)
    {
        builder.Property(e => e.ProgressionPathId).IsRequired();
        builder.Property(e => e.ExpressionId).IsRequired();

        builder
            .HasOne(x => x.ProgressionPath)
            .WithMany(x => x.ProgressionPathAuditTrails)
            .HasForeignKey(x => x.ProgressionPathId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder
            .HasOne(x => x.Expression)
            .WithMany(x => x.ProgressionPathAudits)
            .HasForeignKey(x => x.ExpressionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.ProgressionPathAuditTrails);
    }
}
