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

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019e0f2c-060e-72ff-8afd-f2b98ae4ccf8"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019e0f2c-060e-7529-b7ed-51f395fb4066"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019e0f2c-060e-7708-8a22-84f4420ef4f1"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019e0f2c-060e-704a-bf59-f6430ae8a2a1"),
            Name = nameof(Delete),
        };
        
        public static readonly Permission SeeBetaExpressions = new(ResourceInfo)
        {
            Id = new Guid("019e0f2b-f461-7744-aa66-08f4705ba997"),
            Name = nameof(SeeBetaExpressions),
        };
        
        public static readonly Permission DownloadBooklet = new(ResourceInfo)
        {
            Id = new Guid("019cc765-63cd-7fe5-875e-868b10700919"),
            Name = nameof(DownloadBooklet),
            Description = "Downloads the background info for expression, and all the powers",
        };
    }
}
