using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpMappingModels.Audit;

internal class AssignedXpMappingAuditTrailConfiguration
    : IEntityTypeConfiguration<AssignedXpMappingAuditTrail>
{
    public void Configure(EntityTypeBuilder<AssignedXpMappingAuditTrail> builder)
    {
        builder
            .Property(e => e.AssignedXpMappingId)
            .IsRequired();

        builder
            .HasOne(x => x.AssignedXpMapping)
            .WithMany(x => x.AssignedXpMappingAuditTrails)
            .HasForeignKey(x => x.AssignedXpMappingId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.AssignedXpMappingAuditTrails);
    }
}
