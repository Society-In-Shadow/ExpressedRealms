using Ardalis.SmartEnum;

namespace ExpressedRealms.DB.Models.Creatures.PublishType;

public sealed class CreaturePublishTypeEnum : SmartEnum<CreaturePublishTypeEnum, int>
{
    public string Description { get; set; }

    private CreaturePublishTypeEnum(string name, int id, string description)
        : base(name, id)
    {
        Description = description;
    }

    public static readonly CreaturePublishTypeEnum Public = new("Public", 1, "Visible to anyone in the tool");
    public static readonly CreaturePublishTypeEnum Draft = new(
        "Draft",
        2,
        "Creatures that are being worked on"
    );
    public static readonly CreaturePublishTypeEnum Internal = new(
        "Internal",
        3,
        "Creatures only visible to GO's"
    );
}
