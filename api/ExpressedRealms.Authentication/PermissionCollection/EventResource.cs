using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public partial class Permissions
{
    public static class Event
    {
        private static readonly string ResourceName = nameof(Event);

        // Predefined static instances for each policy
        public static readonly Permission Edit = new(ResourceName, nameof(Edit));
        public static readonly Permission View = new(ResourceName, nameof(View));
        public static readonly Permission Create = new(ResourceName, nameof(Create));
        public static readonly Permission Delete = new(ResourceName, nameof(Delete));
        public static readonly Permission Publish = new(ResourceName, nameof(Publish));

    }
}

