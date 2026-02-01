using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class EventQuestion
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019c1b66-d8ab-764e-81d2-b5543e5b638f"),
            Name = nameof(EventQuestion),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019c1b66-d8ab-7aa3-be4a-c0bf83f76295"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019c1b66-d8ab-7c58-a468-7f9ab5387e18"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019c1b66-d8ab-7732-9f53-a78accf366e8"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019c1b66-d8ab-7381-ae07-b659474c4ea6"),
            Name = nameof(Delete),
        };
    }
}
