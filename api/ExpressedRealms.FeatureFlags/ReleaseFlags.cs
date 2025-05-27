using Ardalis.SmartEnum;

namespace ExpressedRealms.FeatureFlags;

public sealed class ReleaseFlags : SmartEnum<ReleaseFlags, string>
{
    public string Description { get; }

    private ReleaseFlags(string name, string key, string description)
        : base(name, key)
    {
        Description = description;
    }

    public static readonly ReleaseFlags TestReleaseFlag = new(
        "Test Feature Flag",
        "test-feature-flag",
        "This is a test feature flag."
    );
    public static readonly ReleaseFlags MyFavorites = new(
        "My Favorites",
        "my-favorites",
        "Shows Favorites."
    );

    public override string ToString()
    {
        return Name;
    }
}
