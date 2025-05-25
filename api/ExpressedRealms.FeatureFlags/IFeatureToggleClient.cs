namespace ExpressedRealms.FeatureFlags;

public interface IFeatureToggleClient
{
    Task<bool> HasFeatureFlag(ReleaseFlags releaseName);
}