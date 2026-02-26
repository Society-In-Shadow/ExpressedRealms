using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Characters.AssignedXP.AssignedXpTypeModels.Audit;

internal class AssignedXpTypeAuditTrailConfiguration
    : IEntityTypeConfiguration<AssignedXpTypeAuditTrail>
{
    public void Configure(EntityTypeBuilder<AssignedXpTypeAuditTrail> builder)
    {
        builder.Property(e => e.AssignedXpTypeId).IsRequired();

        builder
            .HasOne(x => x.AssignedXpType)
            .WithMany(x => x.AssignedXpTypeAuditTrails)
            .HasForeignKey(x => x.AssignedXpTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.AssignedXpTypeAuditTrails);
    }
}
