using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class PlayerExperience
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019cc77c-a318-703e-8dd6-8852b4c10e15"),
            Name = nameof(PlayerExperience),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019cc77c-a318-759d-8213-e01d86a09f3e"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019cc77c-a318-7e50-82fb-a45c8ca71997"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019cc77c-a318-7159-9e8a-7cf3f8fe7f2d"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019cc77c-a318-7c60-a589-851e6b7203a1"),
            Name = nameof(Delete),
        };
    }
}
