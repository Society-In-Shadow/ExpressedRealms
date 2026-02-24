using ExpressedRealms.DB.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup.Audit;

internal class RolePermissionMappingAuditTrailConfiguration
    : IEntityTypeConfiguration<RolePermissionMappingAuditTrail>
{
    public void Configure(EntityTypeBuilder<RolePermissionMappingAuditTrail> builder)
    {
        builder.ConfigureAuditTrailProperties(user => user.RolePermissionMappingAuditTrails);

        builder
            .Property(e => e.RolePermissionMappingId);

        builder.Property(e => e.RoleId).IsRequired();

        builder.Property(e => e.PermissionId);

        builder
            .HasOne(x => x.RolePermissionMapping)
            .WithMany(x => x.RolePermissionMappingAuditTrails)
            .HasForeignKey(x => x.RolePermissionMappingId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(x => x.Role)
            .WithMany(x => x.UserRoleMappingAuditTrails)
            .HasForeignKey(x => x.RoleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Permission)
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
