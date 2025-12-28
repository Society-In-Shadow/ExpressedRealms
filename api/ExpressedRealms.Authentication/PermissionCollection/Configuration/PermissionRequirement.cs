using ExpressedRealms.Authentication.PermissionCollection.Support;
using Microsoft.AspNetCore.Authorization;

namespace ExpressedRealms.Authentication.PermissionCollection.Configuration;

public class PermissionRequirement : IAuthorizationRequirement
{
    public IPermissionAction Permission { get; set; }

    public PermissionRequirement(IPermissionAction permission)
    {
        Permission = permission;
    }
}
