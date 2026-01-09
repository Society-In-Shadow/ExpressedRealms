using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;

namespace ExpressedRealms.Admin.Repository;

public interface IRolesRepository
{
    Task<List<Role>> GetRolesForListAsync();
    Task<EditRoleDto> GetRoleForEditView(int id);
    Task<Role?> FindRoleAsync(int guid);
    Task<bool> RoleExistsAsync(int id);
}
