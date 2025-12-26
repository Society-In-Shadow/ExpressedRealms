namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public record Permission(ResourceInfo ResourceInfo, string Action) : IPermissionAction
{
    public string Name { get; set; } = Action;
    public string? Description { get; set; }
    public string Key => $"{ResourceInfo.Name.ToLowerInvariant()}.{Action.ToLowerInvariant()}";
};