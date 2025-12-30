using ExpressedRealms.Authentication.PermissionCollection.PermissionManager.PermissionHelper;
using ExpressedRealms.DB;
using ExpressedRealms.DB.Models.Authorization.PermissionResources;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup;
using ExpressedRealms.DB.Models.Authorization.RolePermissionMappingSetup.Audit;
using Microsoft.EntityFrameworkCore;
using Permission = ExpressedRealms.DB.Models.Authorization.Permissions.Permission;

namespace ExpressedRealms.Authentication.PermissionCollection.PermissionManager;

public class PermissionManager(ExpressedRealmsDbContext context) : IPermissionManager
{
    private async Task AddUpdateResourcesAndPermissions(
        IReadOnlyList<PermissionResource> codeSidePermissions
    )
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = await context.Database.BeginTransactionAsync();

            var resources = await context
                .Set<PermissionResource>()
                .Include(x => x.Permissions)
                .ToListAsync();

            var dbResourceIds = resources.Select(x => x.Id).ToHashSet();
            var missingResources = codeSidePermissions
                .Where(x => !dbResourceIds.Contains(x.Id))
                .ToList();

            if (missingResources.Count != 0)
            {
                // GPT was very insistent on this
                var cleanedResources = missingResources
                    .Select(x => new PermissionResource()
                    {
                        Name = x.Name,
                        Id = x.Id,
                        Description = x.Description,
                    })
                    .ToList();
                context.Set<PermissionResource>().AddRange(cleanedResources);
                await context.SaveChangesAsync();
                resources.AddRange(cleanedResources);
            }

            foreach (var resource in resources)
            {
                var codeSideResource = codeSidePermissions.FirstOrDefault(x => x.Id == resource.Id);

                // Deleted resources will be handled outside of this loop
                // There's a bit more to it then meets the eye
                if (codeSideResource is null)
                    continue;

                // update info
                resource.Name = codeSideResource.Name;
                resource.Description = codeSideResource.Description;

                // Add Missing Permissions
                var dbResourcePermissionIds = resource.Permissions.Select(x => x.Id).ToHashSet();
                var missingPermissions = codeSideResource
                    .Permissions.Where(x => !dbResourcePermissionIds.Contains(x.Id))
                    .ToList();

                if (missingPermissions.Count != 0)
                {
                    var cleanedPermissions = missingPermissions.Select(x => new Permission()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Key = x.Key,
                        Description = x.Description,
                        PermissionResourceId = resource.Id,
                    });
                    context.Set<Permission>().AddRange(cleanedPermissions);
                }

                // Update Permissions
                foreach (var permission in resource.Permissions)
                {
                    var codeSidePermission = codeSideResource.Permissions.FirstOrDefault(x =>
                        x.Id == permission.Id
                    );

                    // Deleted resources will be handled outside of this loop
                    // There's a bit more to it then meets the eye
                    if (codeSidePermission is null)
                        continue;

                    permission.Name = codeSidePermission.Name;
                    permission.Description = codeSidePermission.Description;
                    permission.Key = codeSidePermission.Key;
                }
            }

            await context.SaveChangesAsync();

            await tx.CommitAsync();
        });
    }

    private async Task RemoveResourceAndPermissions(
        IReadOnlyList<PermissionResource> codeSidePermissions
    )
    {
        var dbPermissions = await context.Set<Permission>().ToListAsync();

        // Find all permissions that need to be removed
        var codeKeys = codeSidePermissions
            .SelectMany(x => x.Permissions.Select(y => y.Id))
            .ToHashSet();

        var removedPermissions = dbPermissions.Where(db => !codeKeys.Contains(db.Id)).ToList();

        var deletedPermissionIds = removedPermissions.Select(x => x.Id).ToHashSet();

        // Remove reference from audit mapping
        await context
            .Set<RolePermissionMappingAuditTrail>()
            .Where(x =>
                x.PermissionId != null && deletedPermissionIds.Contains(x.PermissionId.Value)
            )
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.PermissionId, (Guid?)null));

        // Remove reference From Roles
        await context
            .Set<RolePermissionMapping>()
            .Where(x => deletedPermissionIds.Contains(x.PermissionId))
            .ExecuteDeleteAsync();

        // Remove Permission(s)
        context.Set<Permission>().RemoveRange(removedPermissions);
        await context.SaveChangesAsync();

        // Remove Resource(s)
        // Do we want a resource that doesn't have an action? - No, that should be a bug
        await context
            .Set<PermissionResource>()
            .Where(x => x.Permissions.Count == 0)
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// This is in here to make sure that the permissions reflects what the codebase needs.
    /// It will automatically add and remove permissions, in addition, it will make sure that the description
    /// stay consistent with the codebase.
    /// </summary>
    public async Task UpdatePermissions()
    {
        var codeSideFlags = PermissionManagerHelper.GetCodeSidePermissions();

        await AddUpdateResourcesAndPermissions(codeSideFlags);
        await RemoveResourceAndPermissions(codeSideFlags);
    }
}
