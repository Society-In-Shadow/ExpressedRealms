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
    
    public static readonly ReleaseFlags ShowReportButtons = new(
        "Show Report Download Buttons",
        "show-report-buttons",
        "Allows one to download the reports"
    );

    public override string ToString()
    {
        return Name;
    }
}
