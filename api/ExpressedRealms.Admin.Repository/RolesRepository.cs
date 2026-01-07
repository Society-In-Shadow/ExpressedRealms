using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Admin.Repository;

internal sealed class RolesRepository(ExpressedRealmsDbContext context) : IRolesRepository
{
    public Task<List<Role>> GetRolesForListAsync()
    {
        return context.Set<Role>().AsNoTracking().ToListAsync();
    }
}
