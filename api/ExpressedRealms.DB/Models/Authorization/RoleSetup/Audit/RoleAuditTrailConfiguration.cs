using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.RoleSetup.Audit;

internal class RoleAuditTrailConfiguration
    : IEntityTypeConfiguration<RoleAuditTrail>
{
    public void Configure(EntityTypeBuilder<RoleAuditTrail> builder)
    {
        builder.ToTable("role_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.RoleAuditTrails);

        builder.Property(e => e.RoleId).HasColumnName("role_id").IsRequired();

    }
}
