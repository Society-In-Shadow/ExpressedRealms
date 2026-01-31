using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.Events.Questions.QuestionTypeSetup;

public sealed class QuestionTypeEnum : SmartEnum<QuestionTypeEnum, int>
{
    public bool IsApproved { get; }
    public bool IsCustomizable { get; }

    private QuestionTypeEnum(string name, int id, bool isApproved, bool isCustomizable)
        : base(name, id)
    {
        IsApproved = isApproved;
        IsCustomizable = isCustomizable;
    }
    
    public static readonly QuestionTypeEnum IsMinorCheck = new("Is Minor", 1, true, false);
    public static readonly QuestionTypeEnum PlayerBadgeNumber = new("Player Badge Number", 2, true, false);
    public static readonly QuestionTypeEnum Text = new("Text", 3, false, true);
    public static readonly QuestionTypeEnum Checkbox = new("Checkbox", 4, false, true);
    public static readonly QuestionTypeEnum IsNewPlayer = new("Is New Player", 5, true, false);
    public static readonly QuestionTypeEnum BroughtNewPlayer = new("Brought New Player", 6, true, false);
}