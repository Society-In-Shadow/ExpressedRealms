using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Character
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019c7898-c8e8-7f38-8837-eead0f81811b"),
            Name = nameof(Character),
        };

        public static readonly Permission Retire = new(ResourceInfo)
        {
            Id = new Guid("019c7899-03f1-7a0b-b3d5-1ddef0b3afcc"),
            Name = nameof(Retire),
        };
    }
}
