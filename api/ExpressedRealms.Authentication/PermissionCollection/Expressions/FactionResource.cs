using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Faction
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019f0362-a536-745c-8a8d-276def1079ea"),
            Name = nameof(Faction),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-7f6d-9392-e0d2f5914f6a"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-7817-b18f-4d2347eb83dd"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-7ff3-be70-6ae0a570c37a"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-75da-a245-afdc982b64d4"),
            Name = nameof(Delete),
        };
    }
}
