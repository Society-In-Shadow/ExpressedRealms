namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public interface IPermissionAction
{
    Guid Id { get; init; }
    ResourceInfo ResourceInfo { get; init; }
    string Name { get; init; }
    string? Description { get; init; }
    string Key { get; }
}