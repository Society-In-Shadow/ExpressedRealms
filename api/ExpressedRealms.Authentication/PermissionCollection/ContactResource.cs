using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class CharacterContacts
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019bfb76-15ef-7cbe-b63e-144b21a3cf59"),
            Name = nameof(CharacterContacts),
        };

        public static readonly Permission Approve = new(ResourceInfo)
        {
            Id = new Guid("019bfb76-15ef-7094-a23f-c28e80954bd4"),
            Name = nameof(Approve),
        };
    }
}
