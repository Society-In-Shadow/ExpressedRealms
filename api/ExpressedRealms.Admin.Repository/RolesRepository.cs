using ExpressedRealms.Admin.Repository.DTOs;
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
    
    public Task<EditRoleDto> GetRoleForEditView(int id)
    {
        return context.Set<Role>()
            .Select(x => new EditRoleDto()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                PermissionIds = x.RolePermissionMappings.Select(y => y.PermissionId).ToList()
            })
            .FirstAsync(x => x.Id == id);
    }

    public async Task<bool> RoleExistsAsync(int id)
    {
        return await context.Set<Role>().AnyAsync(x => x.Id == id);
    }

    public async Task<Role?> FindRoleAsync(int guid)
    {
        return await context.Set<Role>().FindAsync(guid);
    }
}
