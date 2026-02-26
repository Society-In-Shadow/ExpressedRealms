using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup.Audit;

internal class UserRoleMappingAuditTrailConfiguration
    : IEntityTypeConfiguration<UserRoleMappingAuditTrail>
{
    public void Configure(EntityTypeBuilder<UserRoleMappingAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.UserRoleMappingAuditTrails);

        builder.Property(e => e.UserRoleMappingId).IsRequired();
        builder.Property(e => e.RoleId).HasMaxLength(450).IsRequired();
        builder.Property(e => e.UserId).IsRequired();
    }
}
