using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Archetypes
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019d47c7-bcfc-7fd3-bb3e-5d0eb38ee766"),
            Name = nameof(Archetypes),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019d47c7-e4b6-76f8-b4cb-ece9b8473005"),
            Name = nameof(Edit),
        };
        
        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019d47e4-dadc-78ee-a598-2d9420352d4e"),
            Name = nameof(View),
        };
        
        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019d48f8-5d07-7fb0-961f-a7563939b13b"),
            Name = nameof(Create),
        };
        
        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019d48c9-0d7e-7be0-bba9-e0f5aaf6358c"),
            Name = nameof(Delete),
        };
    }
}
