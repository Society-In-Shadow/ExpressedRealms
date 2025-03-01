using ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.Roles;

public class Role : IdentityRole<string>
{
    public virtual List<UserRoleAuditTrail> UserRoleAuditTrails { get; set; } = new();
}
