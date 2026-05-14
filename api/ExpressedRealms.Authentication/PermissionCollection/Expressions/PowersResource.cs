using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Powers
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019dfa39-8195-7417-bd91-fe1ff76a5da1"),
            Name = nameof(Powers),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019dfa39-8195-7192-b9a8-439bcf1b3f2b"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019dfa39-8195-76c7-9fa8-5342c719995b"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019dfa39-8195-7066-b9e0-37918836d502"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019dfa39-8195-7e60-b66d-ca49c44f1ce6"),
            Name = nameof(Delete),
        };

        public static readonly Permission EditModifiers = new(ResourceInfo)
        {
            Id = new Guid("019e1f98-249d-7134-8d04-4109292b5899"),
            Name = nameof(EditModifiers),
        };
    }
}
