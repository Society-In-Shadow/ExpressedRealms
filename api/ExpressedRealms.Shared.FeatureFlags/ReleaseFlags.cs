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

    public static readonly ReleaseFlags ShowMarketingContactUsPage = new(
        "Show Marketing Contact Us Pages",
        "show-marketing-contact-us",
        "Allows one to see the contact us page on the marketing materials"
    );

    public static readonly ReleaseFlags ShowFactionDropdown = new(
        "Show Faction Dropdown on Add / Edit Character",
        "show-faction-dropdown",
        "Allows one to see the faction dropdown on the add / edit character page"
    );

    public static readonly ReleaseFlags ShowAssignedXpPanel = new(
        "Show Assigned XP Panel",
        "show-assigned-xp-panel",
        "Shows how the XP has been assigned to the character"
    );

    public override string ToString()
    {
        return Name;
    }
}
