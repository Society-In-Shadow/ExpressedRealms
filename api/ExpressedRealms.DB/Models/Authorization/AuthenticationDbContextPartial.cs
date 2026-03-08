using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup.Audit;
using ExpressedRealms.DB.Models.Authorization.RoleSetup.Audit;
using ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup.Audit;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace ExpressedRealms.DB;

public partial class ExpressedRealmsDbContext
{
    public DbSet<UserRoleMappingAuditTrail> UserRoleMappingAuditTrails { get; set; }
    public DbSet<RoleAuditTrail> RoleAuditTrails { get; set; }
    public DbSet<RolePermissionMappingAuditTrail> RolePermissionMappingAuditTrails { get; set; }
}
