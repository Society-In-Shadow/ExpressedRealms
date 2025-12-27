using System.Reflection;
using ExpressedRealms.Authentication.PermissionCollection.Support;
using ExpressedRealms.DB.Models.Authorization.PermissionResources;
using Permission = ExpressedRealms.DB.Models.Authorization.Permissions.Permission;

namespace ExpressedRealms.Authentication.PermissionCollection.PermissionManager.PermissionHelper;

public static class PermissionManagerHelper
{
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
    
    public static IReadOnlyList<PermissionResource> GetCodeSidePermissions()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var permissions = assemblies
            .SelectMany(a => a.GetTypes())
            .SelectMany(GetPermissionFields)
            .ToList();

        return permissions.GroupBy(x => x.ResourceInfo)
            .Select(x => new PermissionResource()
        {
            Name = x.Key.Name,
            Id = x.Key.Id,
            Description = x.Key.Description,
            Permissions = x.Select(x => new Permission()
            {
                Id = x.Id,
                Name = x.Name,
                Key = x.Key,
                Description = x.Description,
                PermissionResourceId = x.ResourceInfo.Id
            }).ToList()
        }).ToList();
    }
}