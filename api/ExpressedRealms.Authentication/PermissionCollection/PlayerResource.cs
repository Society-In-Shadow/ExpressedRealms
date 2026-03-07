using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Player
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019bd49e-1d27-7ea9-ac5a-7cee9a57050a"),
            Name = nameof(Player),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019bd49e-1d27-7294-a9c4-ea108de83848"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019bd49e-1d27-784a-861f-0aced5176924"),
            Name = nameof(View),
        };

        public static readonly Permission Disable = new(ResourceInfo)
        {
            Id = new Guid("019bd49e-1d27-72bb-a41d-9f9526be3402"),
            Name = nameof(Disable),
        };

        public static readonly Permission Enable = new(ResourceInfo)
        {
            Id = new Guid("019bd4a3-0c28-70aa-a9b2-a85c833998b1"),
            Name = nameof(Enable),
        };

        public static readonly Permission BypassEmailConfirmation = new(ResourceInfo)
        {
            Id = new Guid("019bd49e-1d27-72d4-a35e-7ea74a5c3b08"),
            Name = nameof(BypassEmailConfirmation),
        };

        public static readonly Permission BypassLockout = new(ResourceInfo)
        {
            Id = new Guid("019bd4f0-de40-71b6-b73c-d5462cf918e1"),
            Name = nameof(BypassLockout),
        };

        public static readonly Permission ManageRoles = new(ResourceInfo)
        {
            Id = new Guid("019bd4f5-b415-7719-abf5-9f9ff11fd7ec"),
            Name = nameof(ManageRoles),
        };

        public static readonly Permission ViewActivityLogs = new(ResourceInfo)
        {
            Id = new Guid("019bd533-a0bb-76fa-b55b-fab62ca68786"),
            Name = nameof(ViewActivityLogs),
        };
    }
}
