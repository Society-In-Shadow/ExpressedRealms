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

    public static readonly ReleaseFlags ShowPowersTab = new(
        "Show Power Tab",
        "show-power-tab",
        "On the expressions, shows the power tab."
    );

    public static readonly ReleaseFlags ShowRuleBook = new(
        "Show Rule Book Nav",
        "show-rule-book-nav",
        "Allows one to see the rule book in the nav bar."
    );

    public static readonly ReleaseFlags ShowTreasuredTales = new(
        "Show Treasure Tales Nav",
        "show-treasured-tales-nav",
        "Allows one to see the treasured tales in the nav bar."
    );

    public static readonly ReleaseFlags EnableKnowledgeManagement = new(
        "Show Knowledges / Enable Management",
        "show-knowledges",
        "Allows one to see and modify the knowledges."
    );
    
    public static readonly ReleaseFlags ShowMarketingPages = new(
        "Show Marketing Pages",
        "show-marketing",
        "Allows one to see the typical landing pages you'd expect from a company."
    );

    public override string ToString()
    {
        return Name;
    }
}
