using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Expression
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019cc765-63cd-7444-88b8-1e8caeced53d"),
            Name = nameof(Expression),
        };

        public static readonly Permission DownloadBooklet = new(ResourceInfo)
        {
            Id = new Guid("019cc765-63cd-7fe5-875e-868b10700919"),
            Name = nameof(DownloadBooklet),
            Description = "Downloads the background info for expression, and all the powers",
        };
    }
}
