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

    public static readonly ReleaseFlags ShowInventoryPage = new(
        "Show Inventory Tab on Rule Book",
        "show-inventory-page",
        "Allows one to see the inventory tab on the rule book page"
    );

    public static readonly ReleaseFlags ShowPowersOnCharacter = new(
        "Show Powers on Character Page",
        "show-character-powers",
        "Allows one to see the powers tab on a character"
    );

    public static readonly ReleaseFlags ShowFactionDropdown = new(
        "Show Faction Dropdown on Add / Edit Character",
        "show-faction-dropdown",
        "Allows one to see the faction dropdown on the add / edit character page"
    );

    public static readonly ReleaseFlags ShowBlessingCMS = new(
        "Shows the purpose built blessing CMS system",
        "show-blessing-cms",
        "Allows one to see blessing CMS system"
    );

    public static readonly ReleaseFlags HideBlessingSections = new(
        "Hides the sections that exist in the blessing CMS system",
        "hide-blessing-sections",
        "Hides the sections that exist in the blessing CMS system"
    );

    public override string ToString()
    {
        return Name;
    }
}
