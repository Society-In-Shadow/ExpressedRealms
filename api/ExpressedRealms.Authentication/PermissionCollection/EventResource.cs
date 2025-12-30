using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Event
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019b5cf8-4d01-75ae-94b1-ea0839af6fa6"),
            Name = nameof(Event),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019b5cf8-4d01-704d-a3e8-5f0629f9b02d"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019b5cf8-4d01-7293-9902-cb6118e658e8"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019b5cf8-4d01-73ea-8bf3-411a5b9896b2"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019b5cf8-4d01-7107-b858-80d77c737bc7"),
            Name = nameof(Delete),
        };

        public static readonly Permission Publish = new(ResourceInfo)
        {
            Id = new Guid("019b5d27-273b-7b47-9610-048730a4169a"),
            Name = nameof(Publish),
            Description =
                "Will publish the event to the public site, create an event in Discord, and publish a messages to the announcements channel.",
        };
    }
}
