using ExpressedRealms.Authentication.PermissionCollection.Support;

namespace ExpressedRealms.Authentication.PermissionCollection;

public static partial class Permissions
{
    public static class Faction
    {
        private static readonly ResourceInfo ResourceInfo = new()
        {
            Id = new Guid("019f0362-a536-745c-8a8d-276def1079ea"),
            Name = nameof(Faction),
        };

        public static readonly Permission Edit = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-7f6d-9392-e0d2f5914f6a"),
            Name = nameof(Edit),
        };

        public static readonly Permission View = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-7817-b18f-4d2347eb83dd"),
            Name = nameof(View),
        };

        public static readonly Permission Create = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-7ff3-be70-6ae0a570c37a"),
            Name = nameof(Create),
        };

        public static readonly Permission Delete = new(ResourceInfo)
        {
            Id = new Guid("019f0362-a536-75da-a245-afdc982b64d4"),
            Name = nameof(Delete),
        };

        public static readonly Permission ApprovePromotion = new(ResourceInfo)
        {
            Id = new Guid("019fb6e7-5e92-741b-b16b-c780e707abfb"),
            Name = nameof(ApprovePromotion),
        };
        
        public static readonly Permission ViewAllParticipants = new(ResourceInfo)
        {
            Id = new Guid("01a0377c-b541-7763-982e-e515d1acff06"),
            Name = nameof(ViewAllParticipants),
        };
    }
}
