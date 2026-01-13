using ExpressedRealms.Admin.Repository.DTOs;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Helpers;
using ExpressedRealms.DB.Models.Authorization.PermissionResources;
using ExpressedRealms.DB.Models.Authorization.Permissions;
using ExpressedRealms.DB.Models.Authorization.RoleSetup;
using ExpressedRealms.DB.Models.Authorization.UserRoleMappingSetup;
using Microsoft.EntityFrameworkCore;

namespace ExpressedRealms.Admin.Repository;

internal sealed class RolesRepository(
    ExpressedRealmsDbContext context,
    CancellationToken cancellationToken
) : IRolesRepository
{
    public Task<List<Role>> GetRolesForListAsync()
    {
        return context.Set<Role>().AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task<EditRoleDto> GetRoleForEditView(int id)
    {
        return context
            .Set<Role>()
            .Select(x => new EditRoleDto()
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                PermissionIds = x.RolePermissionMappings.Select(y => y.PermissionId).ToList(),
            })
            .FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> RoleExistsAsync(int id)
    {
        return await context.Set<Role>().AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<bool> RoleUserMappingExistsAsync(int roleId, string userId)
    {
        return await context
            .Set<UserRoleMapping>()
            .AnyAsync(x => x.RoleId == roleId && x.UserId == userId, cancellationToken);
    }

    public async Task DeleteRoleUserMappingAsync(int roleId, string userId)
    {
        await context
            .Set<UserRoleMapping>()
            .Where(x => x.RoleId == roleId && x.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> RoleNameExistsAsync(string name)
    {
        return await context
            .Set<Role>()
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);
    }

    public async Task<bool> RoleNameExistsAsync(int id, string name)
    {
        return await context
            .Set<Role>()
            .AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != id, cancellationToken);
    }

    public async Task<Role> GetRoleForEditAsync(int guid)
    {
        return await context
            .Set<Role>()
            .Include(x => x.RolePermissionMappings)
            .FirstAsync(x => x.Id == guid, cancellationToken);
    }

    public async Task<List<PermissionResource>> GetPermissionResourcesForList()
    {
        return await context.Set<PermissionResource>().Include(x => x.Permissions).ToListAsync();
    }

    public async Task<int> AddAsync(Role role)
    {
        await context.AddAsync(role);
        await context.SaveChangesAsync(cancellationToken);
        return role.Id;
    }

    public async Task<int> AddUserRoleMappingAsync(UserRoleMapping role)
    {
        await context.AddAsync(role);
        await context.SaveChangesAsync(cancellationToken);
        return role.Id;
    }

    public async Task<List<Guid>> GetInvalidPermissions(List<Guid> permissionIds)
    {
        var validPermissions = await context
            .Set<Permission>()
            .Where(x => permissionIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        return permissionIds.Except(validPermissions).ToList();
    }

    public async Task EditAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await context.CommonSaveChanges(entity, cancellationToken);
    }
}
