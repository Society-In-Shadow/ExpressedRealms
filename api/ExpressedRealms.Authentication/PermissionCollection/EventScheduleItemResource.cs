using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public partial class Permissions
{
    public static class EventScheduleItem
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019b5d79-e3fd-7e14-84f7-4a0a418afe0f"),
            Name = nameof(EventScheduleItem),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019b5d79-e3fd-7682-80e5-15a6123776b8"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019b5d79-e3fd-711a-b80e-a4078a6bd615"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019b5d79-e3fd-7923-9522-deab6ef81672"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019b5d79-e3fd-7102-8356-ba9977b3d3bf"),
            Name = nameof(Delete),
        };
    }
}
