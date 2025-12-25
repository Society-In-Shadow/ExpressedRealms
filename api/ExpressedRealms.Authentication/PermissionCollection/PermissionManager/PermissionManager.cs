using System.Reflection;
using ExpressedRealms.Authentication.PermissionCollection.Support;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Authorization.PermissionResources;
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

        var permissionResources = codeSidePermissions.GroupBy(x => x.ResourceInfo.Name).ToList();
        
        var addedResources = permissionResources
            .Where(g => dbResources.All(r => r.Name != g.Key))
            .Select(g =>
            {
                var resourceInfo = g.First().ResourceInfo;

                var permissions = g.Select(x => new Permission()
                {
                    Name = x.Name,
                    Key = x.Key,
                    Description = x.Description,
                }).ToList();
                
                return new
                {
                    PermissionResource = new PermissionResource
                    {
                        Name = resourceInfo.Name,
                        Description = resourceInfo.Description
                    },
                    Permissions = permissions
                };
            })
            .ToList();

        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await using var tx =
                await _context.Database.BeginTransactionAsync();
            
            if (addedResources.Count != 0)
            {
                _context.Set<PermissionResource>().AddRange(addedResources.Select(x => x.PermissionResource));
            }

            await _context.SaveChangesAsync();

            var addedPermissions = addedResources
                .SelectMany(g => g.Permissions.Select(x => new Permission()
                {
                    Name = x.Name,
                    Description = x.Description,
                    Key = x.Key,
                    PermissionResourceId = g.PermissionResource.Id
                }).ToList()).ToList();

            _context.Set<Permission>().AddRange(addedPermissions);

            await _context.SaveChangesAsync();

            await tx.CommitAsync();
        });

    }
/*
    private async Task RemoveResourceAndPermissions(IReadOnlyList<IPermissionAction> codeSidePermissions, List<PermissionResource> dbResources, List<Permission> dbPermissions)
    {

        // Find all permissions that need to be removed
        var codeKeys = codeSidePermissions
            .Select(x => x.Key)
            .ToHashSet(StringComparer.Ordinal);

        var removedPermissions = dbPermissions
            .Where(db => !codeKeys.Contains(db.Key))
            .ToList();

        var permissionIds = removedPermissions.Select(x => x.Id).ToList();

        // Remove reference from audit mapping

        // Remove reference From Roles

        // Remove Permission(s)
        _context.Set<Permission>().RemoveRange(removedPermissions);
        await _context.SaveChangesAsync();

        // Remove Resource(s)

    }

    private async Task UpdateFeatureFlags(
        List<ReleaseFlags> codeSideFlags,
        List<Flag> hostSideFlags
    )
    {
        var matchingFlags = hostSideFlags.Where(x => codeSideFlags.Any(y => y.Value == x.Key));

        foreach (var matchingFlag in matchingFlags)
        {
            var codeSideFlag = codeSideFlags.First(x => x.Value == matchingFlag.Key);

            if (
                codeSideFlag.Name == matchingFlag.Name
                && codeSideFlag.Description == matchingFlag.Description
            )
                continue;

            matchingFlag.Name = codeSideFlag.Name;
            matchingFlag.Description = codeSideFlag.Description;

            await _fliptRestClient.ApiV1NamespacesFlagsPutAsync(
                "default",
                matchingFlag.Key,
                new UpdateFlagRequest()
                {
                    Name = matchingFlag.Name,
                    Description = matchingFlag.Description,
                    Key = matchingFlag.Key,
                    Enabled = matchingFlag.Enabled,
                    AdditionalProperties = matchingFlag.AdditionalProperties,
                    DefaultVariantId = matchingFlag.DefaultVariant?.Id,
                    Metadata = matchingFlag.Metadata,
                    NamespaceKey = matchingFlag.NamespaceKey,
                }
            );
        }
    }*/

    /// <summary>
    /// This is in here to make sure that the feature flag instance reflects what the codebase needs.
    /// It will automatically add and remove feature flags, in addition, it will make sure that the name and description
    /// stay consistent with the codebase.
    /// </summary>
    public async Task UpdatePermissions()
    {
        var codeSideFlags = GetAllPermissions();
        var resources = await _context.Set<PermissionResource>().ToListAsync();
        var permissions = await _context.Set<Permission>().ToListAsync();

        await AddResourceAndPermissions(codeSideFlags, resources);
        //await RemoveResourceAndPermissions(codeSideFlags, resources, permissions);
        //await UpdateFeatureFlags(codeSideFlags, hostSideFlags);*/
    }
}
