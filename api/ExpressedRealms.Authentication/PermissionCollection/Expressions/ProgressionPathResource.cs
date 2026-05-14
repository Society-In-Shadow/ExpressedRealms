using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class ProgressionPath
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019e0045-49be-78e9-bdaa-3c22012a892c"),
            Name = nameof(ProgressionPath),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019e0045-49be-7fe0-81a4-abb48100f4a0"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019e0045-49be-748a-9bc7-35ff6f9b0e40"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019e0045-49be-7404-9988-d815f7360b42"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019e0045-49be-7f1f-8ad7-986d9796ec59"),
            Name = nameof(Delete),
        };

        public static readonly Permission EditModifiers = new(ResourceInfo)
        {
            Id = new Guid("019e23f9-41e4-7d15-83e1-146b207688a7"),
            Name = nameof(EditModifiers),
        };
    }
}
