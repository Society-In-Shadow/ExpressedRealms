namespace ExpressedRealms.FeatureFlags.FeatureManager.ApiModels;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

internal class Root
{
    public List<Flag> Flags { get; set; }
    public string NextPageToken { get; set; }
    public int TotalCount { get; set; }
}
