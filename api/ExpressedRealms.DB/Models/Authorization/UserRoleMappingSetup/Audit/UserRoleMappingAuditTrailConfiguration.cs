using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup.Audit;

internal class UserRoleMappingAuditTrailConfiguration
    : IEntityTypeConfiguration<UserRoleMappingAuditTrail>
{
    public void Configure(EntityTypeBuilder<UserRoleMappingAuditTrail> builder)
    {
        builder.ToTable("user_role_mapping_audit_trail");

        builder.ConfigureAuditTrailProperties(user => user.UserRoleMappingAuditTrails);

        builder.Property(e => e.UserRoleMappingId).HasColumnName("user_role_mapping_id").IsRequired();
        builder.Property(e => e.RoleId).HasColumnName("role_id").HasMaxLength(450).IsRequired();
        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();

    }
}
