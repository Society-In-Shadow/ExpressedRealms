using OpenFeature.Model;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace ExpressedRealms.FeatureFlags.FeatureManager.ApiModels;

internal class Flag
{
    public string Key { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Enabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Variant> Variants { get; set; }
    public string NamespaceKey { get; set; }
    public string Type { get; set; }
    public DefaultVariant DefaultVariant { get; set; }
    public Metadata Metadata { get; set; }
}