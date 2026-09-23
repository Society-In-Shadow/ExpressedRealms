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
    public static readonly CheckinStageEnum CrbPrinted = new(
        "CRB Printed",
        3,
        "The CRB has been printed, just needs assembly"
    );
    public static readonly CheckinStageEnum CrbReadForPickup = new(
        "CRB Read For Pickup (Depreciated)",
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

    public static readonly CheckinStageEnum CrbAssembled = new(
        "CRB has been Assembled",
        11,
        "CRB is fully assembled, including power cards, strips and badge"
    );

    public static readonly CheckinStageEnum PlayerNeedsReapproval = new(
        "Player Needs Reapproval",
        12,
        "This is used when a player needs to have their CRB re-printed / approved.  Usually due to retirement or sheet changes."
    );

    public static readonly CheckinStageEnum CharacterStorageQuestion = new(
        "Character Storage Question",
        13,
        "Players have the ability to pay for character storage, this step gets that sorted out."
    );

    public static readonly CheckinStageEnum PlayerEarlyCheckin = new(
        "Player Early Checkin",
        14,
        "This is the player opting into early checkin for a convention, they will be recorded as responsible for this step."
    );

    public static readonly CheckinStageEnum FinalStage = new(
        "Final Stage",
        15,
        "The player has completed all steps, and is fully checked in for all the days of the event."
    );

    public static readonly CheckinStageEnum AwaitingInitialCheckin = new(
        "Awaitin Checkin",
        16,
        "The player has not checked in yet for this event"
    );
}
