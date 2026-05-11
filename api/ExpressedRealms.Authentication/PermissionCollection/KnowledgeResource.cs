using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Knowledges
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019e171e-a194-7400-9bb3-1bc39c75c1ba"),
            Name = nameof(Knowledges),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019e171e-a194-799e-8256-9ea1be5fa7d4"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019e171e-a194-7c8d-b81d-3bb1c1add4d9"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019e171e-a194-7c5c-8ee9-993476670d85"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019e171e-a195-76e8-9d0f-e2c846b99950"),
            Name = nameof(Delete),
        };
    }
}
