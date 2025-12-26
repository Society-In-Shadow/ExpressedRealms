using System.Reflection;
using ExpressedRealms.Authentication.PermissionCollection.Support;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Authorization.PermissionResources;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup.Audit;
using Microsoft.EntityFrameworkCore;
using Permission = ExpressedRealms.DB.Models.Authorization.Permissions.Permission;

namespace ExpressedRealms.Authentication.PermissionCollection.PermissionManager;

public class PermissionManager : IPermissionManager
{
    private readonly ExpressedRealmsDbContext _context;

    public PermissionManager(ExpressedRealmsDbContext context)
    {
        _context = context;
    }

    private static IEnumerable<IPermissionAction> GetPermissionFields(Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f =>
                typeof(IPermissionAction)
                    .IsAssignableFrom(f.FieldType))
            .Select(f => f.GetValue(null))
            .OfType<IPermissionAction>();
    }
    
    private static IReadOnlyList<IPermissionAction> GetAllPermissions()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        return assemblies
            .SelectMany(a => a.GetTypes())
            .SelectMany(GetPermissionFields)
            .ToList();
    }

    private async Task AddResourceAndPermissions(IReadOnlyList<IPermissionAction> codeSidePermissions, List<PermissionResource> dbResources)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await _context.Database.BeginTransactionAsync();
            
            var codeResources = codeSidePermissions.GroupBy(x => x.ResourceInfo.Name).ToList();
            
            var existingResourceNames = dbResources
                .Select(r => r.Name)
                .ToHashSet(StringComparer.Ordinal);

            var addedResources = codeResources
                .Where(g => !existingResourceNames.Contains(g.Key))
                .Select(g =>
                {
                    var info = g.First().ResourceInfo;

                    return new PermissionResource
                    {
                        Name = info.Name,
                        Description = info.Description
                    };
                })
                .ToList();
            
            if (addedResources.Count != 0)
            {
                _context.Set<PermissionResource>().AddRange(addedResources);
                await _context.SaveChangesAsync();
            }
            
            var resourceInfo = dbResources
                .Concat(addedResources)
                .ToDictionary(r => r.Name, r => r.Id, StringComparer.Ordinal);
            
            var dbPermissions = await _context.Set<Permission>().Select(x => x.Key).ToHashSetAsync();
            
            var missingPermissions = codeSidePermissions
                .Where(x => !dbPermissions.Contains(x.Key))
                .Select(x => new Permission()
                {
                    Key = x.Key,
                    Name = x.Name,
                    Description = x.Description,
                    PermissionResourceId = resourceInfo[x.ResourceInfo.Name]
                })
                .ToList();

            if (missingPermissions.Count != 0)
            {
                _context.Set<Permission>().AddRange(missingPermissions);
                await _context.SaveChangesAsync();
            }

            await tx.CommitAsync();
        });

    }

    private async Task RemoveResourceAndPermissions(IReadOnlyList<IPermissionAction> codeSidePermissions, List<Permission> dbPermissions)
    {

        // Find all permissions that need to be removed
        var codeKeys = codeSidePermissions
            .Select(x => x.Key)
            .ToHashSet(StringComparer.Ordinal);

        var removedPermissions = dbPermissions
            .Where(db => !codeKeys.Contains(db.Key))
            .ToList();

        var deletedPermissionIds = removedPermissions.Select(x => x.Id).ToList();

        // Remove reference from audit mapping
        await _context.Set<RolePermissionMappingAuditTrail>()
            .Where(x => x.PermissionId != null && deletedPermissionIds.Contains(x.PermissionId.Value))
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.PermissionId, (int?)null));

        // Remove reference From Roles
        await _context.Set<RolePermissionMapping>()
            .Where(x => deletedPermissionIds.Contains(x.Id))
            .ExecuteDeleteAsync();

        // Remove Permission(s)
        _context.Set<Permission>().RemoveRange(removedPermissions);
        await _context.SaveChangesAsync();

        // Remove Resource(s)
        // Do we want a resource that doesn't have an action? - No, that should be a bug
        await _context.Set<PermissionResource>().Where(x => x.Permissions.Count == 0)
            .ExecuteDeleteAsync();

    }

    private async Task UpdateResourceAndPermissions(IReadOnlyList<IPermissionAction> codeSidePermissions)
    {
        var dbPermissions = await _context.Set<Permission>().ToListAsync();
        foreach (var dbPermission in dbPermissions)
        {
            var codeSidePermission = codeSidePermissions.First(x => x.Key == dbPermission.Key);
            dbPermission.Description = codeSidePermission.Description;
        }
        
        var resources = codeSidePermissions
            .GroupBy(x => x.ResourceInfo.Name)
            .Select( x => x.First().ResourceInfo)
            .ToList();
        
        var dbPermissionResource = await _context.Set<PermissionResource>().ToListAsync();
        foreach (var resource in dbPermissionResource)
        {
            var codeSidePermission = resources.First(x => x.Name == resource.Name);
            resource.Description = codeSidePermission.Description;
        }
        
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// This is in here to make sure that the permissions reflects what the codebase needs.
    /// It will automatically add and remove permissions, in addition, it will make sure that the description
    /// stay consistent with the codebase.
    /// </summary>
    public async Task UpdatePermissions()
    {
        var codeSideFlags = GetAllPermissions();
        var resources = await _context.Set<PermissionResource>().ToListAsync();
        var permissions = await _context.Set<Permission>().ToListAsync();

        await AddResourceAndPermissions(codeSideFlags, resources);
        await RemoveResourceAndPermissions(codeSideFlags, permissions);
        await UpdateResourceAndPermissions(codeSideFlags);
    }
}
