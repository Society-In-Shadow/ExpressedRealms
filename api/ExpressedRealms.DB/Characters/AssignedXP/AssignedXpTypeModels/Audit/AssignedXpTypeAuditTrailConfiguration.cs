using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Characters.AssignedXP.AssignedXpTypeModels.Audit;

internal class AssignedXpTypeAuditTrailConfiguration
    : IEntityTypeConfiguration<AssignedXpTypeAuditTrail>
{
    public void Configure(EntityTypeBuilder<AssignedXpTypeAuditTrail> builder)
    {
        builder.ToTable("assigned_xp_type_audit_trail");

        builder.Property(e => e.AssignedXpTypeId).HasColumnName("assigned_xp_type_id").IsRequired();

        builder
            .HasOne(x => x.AssignedXpType)
            .WithMany(x => x.AssignedXpTypeAuditTrails)
            .HasForeignKey(x => x.AssignedXpTypeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.ConfigureAuditTrailProperties(user => user.AssignedXpTypeAuditTrails);
    }
}
