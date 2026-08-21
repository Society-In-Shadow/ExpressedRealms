using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.ModifierSystem.StatModifiers;

public sealed class AssignedXpTypeEnum : SmartEnum<AssignedXpTypeEnum, int>
{
    public string Description { get; set; }

    private AssignedXpTypeEnum(int id, string name, string description)
        : base(name, id)
    {
        Description = description;
    }

    public static readonly AssignedXpTypeEnum EventXp = new(
        1,
        "Event XP",
        "XP that comes from Events that is automatically assigned"
    );
    public static readonly AssignedXpTypeEnum CheckinBonus = new(
        2,
        "Check-in Bonus",
        "XP earned when they initially check in"
    );
    public static readonly AssignedXpTypeEnum AwardedXp = new(
        3,
        "Awarded XP",
        "XP assigned out for best costume, etc"
    );
    public static readonly AssignedXpTypeEnum FirstTimePlayerXp = new(
        4,
        "First Time Player XP",
        "First time players will get max of 5 XP"
    );
    public static readonly AssignedXpTypeEnum BroughtNewPlayerXp = new(
        5,
        "Brought New Player XP",
        "Player introduced new player, will get max XP"
    );
    public static readonly AssignedXpTypeEnum Other = new(
        6,
        "Other",
        "XP is being assigned out for uncommon reason"
    );
    public static readonly AssignedXpTypeEnum BoughtCharacterStorage = new(
        7,
        "Bought Character Storage",
        "When a user pays for character storage, they get max of 5 XP"
    );
}
