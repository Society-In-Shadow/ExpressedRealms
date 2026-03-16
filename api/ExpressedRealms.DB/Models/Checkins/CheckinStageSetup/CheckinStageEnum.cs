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
    
    public static readonly CheckinStageEnum AgeCheckApproval = new(
        "Age Check Approval",
        8,
        "User has completed the age check approval process, or player has been previously approved as an adult"
    );
    
    public static readonly CheckinStageEnum EventQuestionsCheck = new(
        "Event Questions Check",
        9,
        "Event Question have been answered"
    );
    
    public static readonly CheckinStageEnum AssignedXpCheck = new(
        "Assign XP Check",
        10,
        "Player has been assigned XP"
    );
    
    public static readonly CheckinStageEnum PrintedCrb = new(
        "CRB has been printed",
        11,
        "CRB has printed at least once during this event"
    );
}
