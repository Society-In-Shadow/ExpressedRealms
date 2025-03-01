using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.UserRoles;

[AuditInclude]
public class UserRole : IdentityUserRole<string> { }
