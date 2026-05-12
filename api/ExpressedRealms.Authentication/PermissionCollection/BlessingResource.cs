using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Blessings
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019e1a74-01af-75e1-a495-6373a4ede266"),
            Name = nameof(Blessings),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019e1a74-01af-72bc-b1a5-5fe42e6b2e4b"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019e1a74-01af-7e93-8566-991f7fdb8c3a"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019e1a74-01af-78b5-a8fb-3103f1131c97"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019e1a74-01af-7ed3-9a73-b493e6842dc6"),
            Name = nameof(Delete),
        };
    }
}
