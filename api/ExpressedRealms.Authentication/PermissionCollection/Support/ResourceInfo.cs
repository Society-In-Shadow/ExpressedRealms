namespace ExpressedRealms.Authentication.PermissionCollection.Support;

public record ResourceInfo()
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
};