using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class DevDebug
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019bcdf8-05cd-78f5-ac37-6981d013a37f"),
            Name = nameof(DevDebug),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019bcdf7-f3ed-7e49-b340-6cc1fcb8ff3d"),
            Name = nameof(View),
            Description = "View the test page in admin menu.",
        };

        public static readonly Permission SendTestEmail = new(ResourceInfo)
        {
            Id = new Guid("019bcdf8-05cd-760f-bc73-6b69de77e250"),
            Name = nameof(SendTestEmail),
            Description = "Sends a test email to the user's email address.",
        };

        public static readonly Permission GetFeatureFlag = new(ResourceInfo)
        {
            Id = new Guid("019bcdf8-05cd-7180-a6aa-0de55eff86a9"),
            Name = nameof(GetFeatureFlag),
            Description = "Tries to get the value of the Test Release Flag",
        };

        public static readonly Permission SendDiscordMessage = new(ResourceInfo)
        {
            Id = new Guid("019bcdf8-05cd-7cf5-83d1-33c1a08d4f03"),
            Name = nameof(SendDiscordMessage),
            Description = "Sends a test message to the Dev Testing Discord channel.",
        };

        public static readonly Permission TestRedis = new(ResourceInfo)
        {
            Id = new Guid("019bcdf8-05cd-70d4-a70a-511fbe70c962"),
            Name = nameof(TestRedis),
            Description =
                "Adds then reads \"TestKey\" to / from redis forcibly.  Bypasses resiliancy",
        };

        public static readonly Permission RunSpecialScripts = new(ResourceInfo)
        {
            Id = new Guid("019c40b9-9547-728c-a9b4-d5d38dcccbd6"),
            Name = nameof(RunSpecialScripts),
            Description = "This allows one to run one off scripts that need to be run via code",
        };
    }
}
