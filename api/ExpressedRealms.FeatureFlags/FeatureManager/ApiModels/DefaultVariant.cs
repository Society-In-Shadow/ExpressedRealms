namespace ExpressedRealms.FeatureFlags.FeatureManager.ApiModels;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

internal class DefaultVariant
{
    public string Id { get; set; }
    public string FlagKey { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Attachment { get; set; }
    public string NamespaceKey { get; set; }
}
