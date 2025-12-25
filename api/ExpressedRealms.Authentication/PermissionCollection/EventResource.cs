using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public partial class Permissions
{
    public static class Event
    {
        private static readonly ResourceInfo ResourceInfo = new(nameof(Event));

        // Predefined static instances for each policy
        public static readonly Permission Edit = new(ResourceInfo, nameof(Edit));
        public static readonly Permission View = new(ResourceInfo, nameof(View));
        public static readonly Permission Create = new(ResourceInfo, nameof(Create));
        public static readonly Permission Delete = new(ResourceInfo, nameof(Delete));
        public static readonly Permission Publish = new(ResourceInfo, nameof(Publish));

    }
}

