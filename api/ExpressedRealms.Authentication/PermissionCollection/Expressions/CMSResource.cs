using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class ContentManagementSystem
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019cc765-63cd-7f5d-879a-30003d1ca1f8"),
            Name = nameof(ContentManagementSystem),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019e0f4e-29ac-7f4a-9c75-3bec9d4941e1"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019e0f4e-29ac-7136-9dc5-17bbf3b339fc"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019e0f4e-29ac-7752-ac4d-69e881915425"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019e0f4e-29ac-7d16-af88-a0606e332a8d"),
            Name = nameof(Delete),
        };

        public static readonly Permission DownloadReport = new(ResourceInfo)
        {
            Id = new Guid("019cc765-63cd-75aa-bebb-fa1e3a11fa85"),
            Name = nameof(DownloadReport),
        };
    }
}
