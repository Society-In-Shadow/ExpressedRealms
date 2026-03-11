using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.UserProfile.PlayerDBModels.PlayerAgeGroupSetup;

public sealed class PlayerAgeGroupEnum : SmartEnum<PlayerAgeGroupEnum, int>
{
    private PlayerAgeGroupEnum(string name, int id)
        : base(name, id) { }

    public static readonly PlayerAgeGroupEnum Child = new("Child (<13)", 1);
    public static readonly PlayerAgeGroupEnum Teen = new("Teen (13-17)", 2);
    public static readonly PlayerAgeGroupEnum Adult = new("Adult (18+)", 3);
}
