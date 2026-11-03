namespace ExpressedRealms.Global.API.Search.Endpoints.GlobalSearch;

public record SearchResultDto()
{
    public int EntryId { get; init; }
    public string Section { get; init; } = null!;
    public string NavName { get; init; } = null!;
    public string Slug { get; init; } = null!;
    public string SectionName { get; init; } = null!;
    public string SectionContent { get; init; } = null!;
    public string? SectionSubSection { get; set; }
};
