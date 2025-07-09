using ExpressedRealms.Authentication.AzureKeyVault;
using ExpressedRealms.Authentication.AzureKeyVault.Secrets;
using Flipt.Rest;

namespace ExpressedRealms.FeatureFlags.FeatureManager;

public class FeatureToggleManager : IFeatureToggleManager
{
    private readonly IKeyVaultManager _keyVaultManager;
    private FliptRestClient _fliptRestClient = null!;

    public FeatureToggleManager(IKeyVaultManager keyVaultManager)
    {
        _keyVaultManager = keyVaultManager;
    }

    private async Task SetupClient()
    {
        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(
            await _keyVaultManager.GetSecret(FeatureFlagSettings.FeatureFlagUrl),
            UriKind.RelativeOrAbsolute
        );
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer ");
        _fliptRestClient = new FliptRestClient(httpClient);
    }

    private async Task<List<Flag>> GetFeatureFlags()
    {
        var flags = await _fliptRestClient.ApiV1NamespacesFlagsGetAsync("default");
        return flags.Flags.ToList();
    }

    private async Task AddFeatureFlags(List<ReleaseFlags> codeSideFlags, List<Flag> hostSideFlags)
    {
        var addedFlags = codeSideFlags.Where(x => !hostSideFlags.Any(y => y.Key == x.Value));
        foreach (var addedFlag in addedFlags)
        {
            await _fliptRestClient.ApiV1NamespacesFlagsPostAsync(
                "default",
                new CreateFlagRequest()
                {
                    Name = addedFlag.Name,
                    Key = addedFlag.Value,
                    Description = addedFlag.Description,
                    Type = CreateFlagRequestType.BOOLEAN_FLAG_TYPE,
                    Enabled = false,
                }
            );
        }
    }

    private async Task RemoveFeatureFlags(
        List<ReleaseFlags> codeSideFlags,
        List<Flag> hostSideFlags
    )
    {
        var removedFlags = hostSideFlags.Where(x => !codeSideFlags.Any(y => y.Value == x.Key));
        foreach (var removedFlag in removedFlags)
        {
            await _fliptRestClient.ApiV1NamespacesFlagsDeleteAsync("default", removedFlag.Key);
        }
    }

    private async Task UpdateFeatureFlags(
        List<ReleaseFlags> codeSideFlags,
        List<Flag> hostSideFlags
    )
    {
        var matchingFlags = hostSideFlags.Where(x => codeSideFlags.Any(y => y.Value == x.Key));

        foreach (var matchingFlag in matchingFlags)
        {
            var codeSideFlag = codeSideFlags.First(x => x.Value == matchingFlag.Key);

            if (
                codeSideFlag.Name == matchingFlag.Name
                && codeSideFlag.Description == matchingFlag.Description
            )
                continue;

            matchingFlag.Name = codeSideFlag.Name;
            matchingFlag.Description = codeSideFlag.Description;

            await _fliptRestClient.ApiV1NamespacesFlagsPutAsync(
                "default",
                matchingFlag.Key,
                new UpdateFlagRequest()
                {
                    Name = matchingFlag.Name,
                    Description = matchingFlag.Description,
                    Key = matchingFlag.Key,
                    Enabled = matchingFlag.Enabled,
                    AdditionalProperties = matchingFlag.AdditionalProperties,
                    DefaultVariantId = matchingFlag.DefaultVariant?.Id,
                    Metadata = matchingFlag.Metadata,
                    NamespaceKey = matchingFlag.NamespaceKey,
                }
            );
        }
    }

    /// <summary>
    /// This is in here to make sure that the feature flag instance reflects what the codebase needs.
    /// It will automatically add and remove feature flags, in addition, it will make sure that the name and description
    /// stay consistent with the codebase.
    /// </summary>
    public async Task UpdateFeatureToggles()
    {
        await SetupClient();

        var codeSideFlags = ReleaseFlags.List.ToList();
        var hostSideFlags = await GetFeatureFlags();

        await AddFeatureFlags(codeSideFlags, hostSideFlags);
        await RemoveFeatureFlags(codeSideFlags, hostSideFlags);
        await UpdateFeatureFlags(codeSideFlags, hostSideFlags);
    }
}
