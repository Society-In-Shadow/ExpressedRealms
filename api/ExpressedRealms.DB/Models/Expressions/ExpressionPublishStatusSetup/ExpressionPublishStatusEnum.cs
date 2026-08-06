using System.Diagnostics.CodeAnalysis;
using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.Expressions.ExpressionPublishStatusSetup;

[SuppressMessage("Reliability", "S3453", Justification = "This is a smart enum")]
public sealed class ExpressionPublishStatusEnum : SmartEnum<ExpressionPublishStatusEnum, int>
{
    public string Description { get; set; }

    private ExpressionPublishStatusEnum(string name, int id, string description)
        : base(name, id)
    {
        Description = description;
    }

    public static readonly ExpressionPublishStatusEnum Published = new(
        "Published",
        1,
        "This has been released to all users"
    );
    public static readonly ExpressionPublishStatusEnum Beta = new(
        "Beta",
        2,
        "This is available to users with beta test permissions"
    );
    public static readonly ExpressionPublishStatusEnum Draft = new(
        "Draft",
        3,
        "This is actively being worked on and only visible to editors"
    );
    public static readonly ExpressionPublishStatusEnum PlayTesting = new(
        "Playtesting",
        4,
        "Everyone can view and create characters with this expression, but cannot be used as a primary character"
    );
}
