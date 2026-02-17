using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.Checkins.CheckinStageSetup;

public sealed class CheckinStageEnum : SmartEnum<CheckinStageEnum, int>
{
    public string Description { get; }

    private CheckinStageEnum(string name, int id, string description)
        : base(name, id)
    {
        Description = description;
    }

    public static readonly CheckinStageEnum ShqApproval = new(
        "SHQ Approval",
        1,
        "All HR Questions have been answered and XP has been assigned out"
    );
    public static readonly CheckinStageEnum GoApproval = new(
        "GO Approval",
        2,
        "GO has reviewed the character and approved it to good for play."
    );
    public static readonly CheckinStageEnum CrbCreation = new(
        "CRB Creation",
        3,
        "SHQ has received that it needs to print and ready the CRB."
    );
    public static readonly CheckinStageEnum CrbReadForPickup = new(
        "CRB Read For Pickup",
        4,
        "Player can now stop by SHQ  to pick up the CRB"
    );
    public static readonly CheckinStageEnum CrbPickedUp = new(
        "CRB Picked Up",
        5,
        "Player has picked up the CRB and verified that it's good to go"
    );
    public static readonly CheckinStageEnum Day2Checkin = new(
        "Day 2 Checkin",
        6,
        "Player has checked in for the day 2 activities (Usually Saturday)"
    );

    public static readonly CheckinStageEnum Day3Checkin = new(
        "Day 3 Checkin",
        7,
        "Player has checked in for the day 3 activities (Usually Sunday)"
    );
}
