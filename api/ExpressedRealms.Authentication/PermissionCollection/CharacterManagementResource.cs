using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class CharacterManagement
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019c7898-c8e8-7f38-8837-eead0f81811b"),
            Name = nameof(CharacterManagement),
        };

        public static readonly Permission Retire = new(ResourceInfo)
        {
            Id = new Guid("019c7899-03f1-7a0b-b3d5-1ddef0b3afcc"),
            Name = nameof(Retire),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019cc6fc-6983-73b9-b243-cbc7e7bd9d56"),
            Name = nameof(View),
            Description = "Allows one to view Manage Characters tab in admin",
        };

        public static readonly Permission ViewCharacterSheet = new(ResourceInfo)
        {
            Id = new Guid("019cc77c-8ea7-7fb1-980b-2d073d576c48"),
            Name = nameof(ViewCharacterSheet),
            Description =
                "Allows one to pull up the primary character sheet for a user and download CRBs",
        };
    }
}
