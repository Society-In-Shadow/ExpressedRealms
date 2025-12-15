using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public partial class Permissions
{
    public static class EventScheduleItem
    {
        private static readonly string ResourceName = nameof(EventScheduleItem);

        // Predefined static instances for each policy
        public static readonly Permission Edit = new(ResourceName, nameof(Edit));
        public static readonly Permission View = new(ResourceName, nameof(View));
        public static readonly Permission Create = new(ResourceName, nameof(Create));
        public static readonly Permission Delete = new(ResourceName, nameof(Delete));

    }
}

