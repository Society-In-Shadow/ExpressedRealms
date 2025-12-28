namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public record Permission(ResourceInfo ResourceInfo) : IPermissionAction
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string Key => $"{ResourceInfo.Name.ToLowerInvariant()}.{Name.ToLowerInvariant()}";
};
