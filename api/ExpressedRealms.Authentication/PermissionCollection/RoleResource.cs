using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Role
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019b95b5-ccbc-7701-b587-56698b7132db"),
            Name = nameof(Role),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019b95b5-ccbc-726e-9fdc-717c62e09afd"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019b95b5-ccbc-7c68-8899-b4e5567d9ca8"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019b95b5-ccbc-7155-bf58-152ee32ecffd"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019b95b5-ccbc-7715-813c-b2e65d5f9509"),
            Name = nameof(Delete),
        };

        public static readonly Permission Assign = new(ResourceInfo)
        {
            Id = new Guid("019bb590-16d4-710b-97f2-0414e5a62e71"),
            Name = nameof(Assign),
        };
    }
}
