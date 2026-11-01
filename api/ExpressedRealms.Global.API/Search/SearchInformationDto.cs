namespace ExpressedRealms.Global.API.Search;

public record SearchInformationDto()
{
    public int EntryId { get; init; }
    public string Section { get; init; } = null!;
    public string NavName { get; init; } = null!;
    public string SectionName { get; init; } = null!;
    public string SectionContent { get; init; } = null!;
};