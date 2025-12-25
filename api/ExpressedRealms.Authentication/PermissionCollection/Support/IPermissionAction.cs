namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public interface IPermissionAction
{
    ResourceInfo ResourceInfo { get; init; }
    string Action { init; }
    string Name { get; set; }
    string? Description { get; set; }
    string Key { get; }
}