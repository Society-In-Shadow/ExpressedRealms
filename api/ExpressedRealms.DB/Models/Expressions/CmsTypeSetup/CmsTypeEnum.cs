using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.Expressions.CmsTypeSetup;

public sealed class CmsTypeEnum : SmartEnum<CmsTypeEnum, int>
{
    public string Description { get; set; }

    private CmsTypeEnum(string name, int id, string description)
        : base(name, id)
    {
        Description = description;
    }

    public static readonly CmsTypeEnum Expression = new("Expression", 1, "Expression Menu Item");
    public static readonly CmsTypeEnum RuleBook = new("Rule Book Section", 13, "Sections that should show up in the rule book.");
    public static readonly CmsTypeEnum WorldBackground = new("World Background Section", 14, "Sections that should show up in the world background.");
}
