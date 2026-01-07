using ExpressedRealms.DB.Models.Authorization.RoleSetup;

namespace ExpressedRealms.Admin.Repository;

public interface IRolesRepository
{
    Task<List<Role>> GetRolesForListAsync();
}
