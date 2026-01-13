using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.DB.Models.Authorization.PermissionResources;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup;
using ExpressedRealms.DB.Shared;
using ExpressedRealms.Shared;

namespace ExpressedRealms.Admin.Repository;

public interface IRolesRepository : IGenericRepository
{
    Task<List<Role>> GetRolesForListAsync();
    Task<EditRoleDto> GetRoleForEditView(int id);
    Task<Role> GetRoleForEditAsync(int guid);
    Task<bool> RoleExistsAsync(int id);
    Task<bool> RoleNameExistsAsync(string name);
    Task<bool> RoleNameExistsAsync(int id, string name);
    Task<int> AddAsync(Role role);
    Task<List<Guid>> GetInvalidPermissions(List<Guid> permissionIds);
    Task<List<PermissionResource>> GetPermissionResourcesForList();
    Task<bool> RoleUserMappingExistsAsync(int roleId, string userId);
    Task<int> AddUserRoleMappingAsync(UserRoleMapping role);
    Task DeleteRoleUserMappingAsync(int roleId, string userId);
    Task<List<GenericListDto<int>>> GetRoleSummaryForListAsync();
}
