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

        public static readonly Permission DownloadReport = new(ResourceInfo)
        {
            Id = new Guid("019cc765-63cd-75aa-bebb-fa1e3a11fa85"),
            Name = nameof(DownloadReport),
        };

    }
}
