namespace ExpressedRealms.FeatureFlags;

public class FeatureFlags
{
    public string Name { get; }
    
    private FeatureFlags(string name)
    {
        Name = name;
    }

    public static readonly FeatureFlags TestFeatureFlag = new(nameof(TestFeatureFlag));

    public override string ToString()
    {
        return Name;
    }
}
