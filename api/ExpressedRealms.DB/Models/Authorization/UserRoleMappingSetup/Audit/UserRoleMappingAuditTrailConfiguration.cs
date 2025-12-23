using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup.Audit;

internal class UserRoleMappingAuditTrailConfiguration
    : IEntityTypeConfiguration<UserRoleMappingAuditTrail>
{
    public void Configure(EntityTypeBuilder<UserRoleMappingAuditTrail> builder)
    {
        builder.ToTable("role_permission_mapping_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.UserRoleMappingAuditTrails);

        builder.Property(e => e.UserRoleMappingId).HasColumnName("blessing_level_id").IsRequired();
        builder.Property(e => e.RoleId).HasColumnName("role_id").IsRequired();
        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

    }
}
