namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public record Permission(ResourceInfo ResourceInfo, string Action) : IPermissionAction
{
    public string Name { get; set; } = Action;
    public string? Description { get; set; }
    public string Key => $"{Action.ToLowerInvariant()}.{ResourceInfo.Name.ToLowerInvariant()}";
};