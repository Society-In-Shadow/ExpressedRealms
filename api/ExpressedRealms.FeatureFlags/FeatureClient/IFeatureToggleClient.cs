namespace ExpressedRealms.FeatureFlags.FeatureClient;

public interface IFeatureToggleClient
{
    Task<bool> HasFeatureFlag(ReleaseFlags releaseName);
}
