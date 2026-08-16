using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatGroupMappings.Audit;

internal class StatGroupMappingAuditTrailConfiguration : IEntityTypeConfiguration<StatGroupMappingAuditTrail>
{
    public void Configure(EntityTypeBuilder<StatGroupMappingAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.StatGroupMappingAuditTrails);

        builder.Property(e => e.StatGroupMappingId).IsRequired();

        builder
            .HasOne(x => x.StatGroupMapping)
            .WithMany(x => x.StatGroupMappingAuditTrails)
            .HasForeignKey(x => x.StatGroupMappingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
