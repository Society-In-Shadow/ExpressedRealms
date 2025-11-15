namespace ExpressedRealms.Characters.API.AssignedXp.Get;

public class AssignedXpInfo
{
    public int Id { get; set; }
    public BasicInfo Event { get; set; } = null!;
    public BasicInfo Character { get; set; } = null!;
    public BasicInfo XpType { get; set; } = null!;
    public BasicInfo Assigner { get; set; } = null!;
    public BasicInfo Player { get; set; } = null!;
    public int Amount { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset DateAssigned { get; set; }
}