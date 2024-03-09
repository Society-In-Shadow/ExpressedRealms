using Microsoft.AspNetCore.Identity;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels;

public class User : IdentityUser
{
    public virtual Player Player { get; set; } = null!;
}
