using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.RoleSetup.Audit;

internal class RoleAuditTrailConfiguration : IEntityTypeConfiguration<RoleAuditTrail>
{
    public void Configure(EntityTypeBuilder<RoleAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.RoleAuditTrails);

        builder.Property(e => e.RoleId).IsRequired();
    }
}
